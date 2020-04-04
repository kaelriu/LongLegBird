using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
#region Initiate Game
    private GameManager _gameManager=null;

    private float _time;

    private float _basewidth=1920.0f;
    private float _baseheight=1080.0f;

    void Awake()
    {
        _rectTr=this.GetComponentInChildren<RectTransform>();
        float a=_baseheight/_basewidth;
        float b=_rectTr.sizeDelta.y/_rectTr.sizeDelta.x;
        float GameScale=a/b;

        GameManager.Instance.Initialize(this);

        _gameManager=GameManager.Instance;

        GameObject newGround =Instantiate(_ground, new Vector3(0.0f,-2.26f,1.0f), transform.rotation) as GameObject;
        newGround.transform.localScale*=GameScale;        
        newGround.transform.SetParent(this.transform);

        GameObject newSky=Instantiate(_sky, new Vector3(0.0f,0.0f,50.0f), transform.rotation) as GameObject;
        newSky.transform.localScale*=GameScale;
        newSky.transform.SetParent(this.transform);

        SetUI();
    }

    public GameObject _player;
    public GameObject _ground;
    public GameObject _obstacle;
    public GameObject _scoreBlock;
    public GameObject _sky;
    private GameObject _playPan;
    private Text _score;
    private RectTransform _rectTr=null, _titleTr=null, _pauseTr=null, _scoreTr=null;
    private IEnumerator _updateUIIEumerator=null;
    public void ReadytoPlay()
    {
        _playPan=new GameObject("PlayPan");

        _playPan.transform.SetParent(this.transform);

        GameObject newPlayer =Instantiate(_player, new Vector3(-6.0f,5.0f,-1.0f), transform.rotation) as GameObject;
        newPlayer.GetComponent<Player>().Initialize();
        newPlayer.transform.SetParent(_playPan.transform);

        _time=-1.0f;
        _gameManager.StartGame();
        
        if(_updateUIIEumerator!=null)
        {
            StopCoroutine(_updateUIIEumerator);
            _updateUIIEumerator=null;
        }

        _updateUIIEumerator=UpdateUI();
        StartCoroutine(_updateUIIEumerator);
        _titleTr.gameObject.SetActive(false);
        _score.gameObject.SetActive(true);
        _scoreTr.gameObject.SetActive(false);
    }

    private IEnumerator UpdateUI()
	{
		while(true)
		{
			_score.text=string.Format("Score: {0}", _gameManager.ReturnScore());

			yield return null;
		}
	}
#endregion

#region UI
    private void SetUI()
    {
        SetTitlePage();
        SetPauseButton();
        SetPausePage();
        SetScorePage();
        _score=_rectTr.Find("Score").GetComponent<Text>();
        _score.text=string.Format("Score: {0}", _gameManager.ReturnScore());
        _score.gameObject.SetActive(false);
    }

    private void SetTitlePage()
    {
        if(_titleTr==null) _titleTr=_rectTr.Find("TitlePage").GetComponent<RectTransform>();
        _titleTr.GetComponent<Button>().onClick.RemoveAllListeners();
        _titleTr.GetComponent<Button>().onClick.AddListener(()=>{
            ReadytoPlay();
        });
        _titleTr.gameObject.SetActive(true);
        
    }
    private bool _pauseGame=false;
    private void PauseGame()
    {
        _pauseGame=true;
        _gameManager.PauseGame(_pauseGame);
        _pauseTr.gameObject.SetActive(_pauseGame);
        Time.timeScale=0.0f;
    }

    private void ResumeGame()
    {
        _pauseGame=false;
        _gameManager.PauseGame(_pauseGame);
        _pauseTr.gameObject.SetActive(_pauseGame);
        Time.timeScale=1.0f;
    }

    private void SetPauseButton()
    {
        Button pauseButton=_rectTr.Find("PauseButton").GetComponent<Button>();
        pauseButton.onClick.RemoveAllListeners();
        pauseButton.onClick.AddListener(()=>{
            this.PauseGame();    
        });
    }

    private void SetPausePage()
    {
        if(_pauseTr==null) _pauseTr=_rectTr.Find("PausePage").GetComponent<RectTransform>();
        _pauseTr.GetComponent<Button>().onClick.RemoveAllListeners();
        _pauseTr.GetComponent<Button>().onClick.AddListener(()=>{
            ResumeGame();
        });
        _pauseTr.gameObject.SetActive(false);
    }

    private void RestartGame()
    {
        Destroy(_playPan);
        ReadytoPlay();
    }
    private void BacktoTitle()
    {
        Destroy(_playPan);
        _gameManager._difficulty=0;
        _obstacleVelocity.x=-8.0f;
        _minRange=0.8f;
        _maxRange=1.4f;
        _titleTr.gameObject.SetActive(true);
        _score.gameObject.SetActive(false);
        _scoreTr.gameObject.SetActive(false);
    }
    private void SetScorePage()
    {
        if(_scoreTr==null) _scoreTr=_rectTr.Find("ScorePage").GetComponent<RectTransform>();
        
        Button restartButton=_scoreTr.Find("Restart").GetComponent<Button>();
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(()=>{
            this.RestartGame();
        });
        
        Button titleButton=_scoreTr.Find("BacktoTitle").GetComponent<Button>();
        titleButton.onClick.RemoveAllListeners();
        titleButton.onClick.AddListener(()=>{
            this.BacktoTitle();
        });

        _scoreTr.gameObject.SetActive(false);
    }
#endregion

#region GAME LOGIC
    private float _obsTime=1.2f;
    private float _minRange=0.8f;
    private float _maxRange=1.4f;
    private Vector2 _obstacleVelocity=new Vector2(-8.0f, 0.0f);
    private int _randomFlag=1;
    void Update()
    {
        
        _time+=Time.deltaTime;
        int flag;

        if(_time>_obsTime&&_gameManager.NowGame())          
        {
            if(_randomFlag<6) flag=_randomFlag++;   //start phase
            else                                    //random phase
            {
                flag=Random.Range(1,6);
                if(++_gameManager._difficulty%5==0)
                {
                    _obstacleVelocity.x-=0.6f;
                    _minRange-=0.04f;
                    _maxRange-=0.02f;
                }
            }

            for(int i=0;i<flag;i++)
            {
                SpawnObstacle(new Vector3(14.0f,-1.0f+1.0f*i,10.0f));
            }

            SpawnScoreBlock(new Vector3(14.0f,0.0f,10.0f));

            _time=0.0f;
            _obsTime=Random.Range(_minRange,_maxRange);
            
            
        }
    }

    private void SpawnObstacle(Vector3 NewPosition)
    {
        GameObject newObstacle =Instantiate(_obstacle, NewPosition, transform.rotation) as GameObject;
        newObstacle.GetComponent<Block>().Initialize(_obstacleVelocity);
        newObstacle.transform.SetParent(_playPan.transform);
    }

    private void SpawnScoreBlock(Vector3 NewPosition)
    {
        GameObject newScoreBlock =Instantiate(_scoreBlock, NewPosition, transform.rotation) as GameObject;
        newScoreBlock.GetComponent<ScoreBlock>().Initialize(_obstacleVelocity);
        newScoreBlock.transform.SetParent(_playPan.transform);
    }

    public void ShowScorePage(int HighScore)
    {
        _scoreTr.gameObject.SetActive(true);
        Text highScoretext=_scoreTr.Find("HighScoreText").GetComponent<Text>();
        highScoretext.text=string.Format("HighScore: {0}", HighScore);
    }

#endregion
}
