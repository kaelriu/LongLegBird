using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
#region Initiate Game
    private GameManager _gameManager=null;

    private float _time;

    void Awake()
    {
        GameManager.Instance.Initialize();

        _gameManager=GameManager.Instance;

        GameObject newGround =Instantiate(_ground, new Vector3(0.0f,-2.26f,1.0f), transform.rotation) as GameObject;        
        newGround.transform.SetParent(this.transform);

        GameObject newSky=Instantiate(_sky, new Vector3(0.0f,0.0f,50.0f), transform.rotation) as GameObject;
        newSky.transform.SetParent(this.transform);

        _rectTr=this.GetComponentInChildren<RectTransform>();

        SetUI();
    }

    public GameObject _player;
    public GameObject _ground;
    public GameObject _obstacle;
    public GameObject _scoreBlock;
    public GameObject _sky;
    private GameObject _playPan;
    private Text _score;
    private RectTransform _rectTr=null;
    private RectTransform _titleTr=null;
    private RectTransform _popupTr=null;
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
        _popupTr.gameObject.SetActive(_pauseGame);
        Time.timeScale=0.0f;
    }

    private void ResumeGame()
    {
        _pauseGame=false;
        _gameManager.PauseGame(_pauseGame);
        _popupTr.gameObject.SetActive(_pauseGame);
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
        if(_popupTr==null) _popupTr=_rectTr.Find("PausePage").GetComponent<RectTransform>();
        _popupTr.GetComponent<Button>().onClick.RemoveAllListeners();
        _popupTr.GetComponent<Button>().onClick.AddListener(()=>{
            ResumeGame();
        });
        _popupTr.gameObject.SetActive(false);
    }
#endregion

    void Update()
    {
        
        _time+=Time.deltaTime;

        if(_time>1.2f&&_gameManager.NowGame())
        {
            int flag=Random.Range(1,6);
            for(int i=0;i<flag;i++)
            {
                GameObject newObstacle =Instantiate(_obstacle, new Vector3(14.0f,-1.0f+1.0f*i,10.0f), transform.rotation) as GameObject;
                newObstacle.GetComponent<Block>().Initialize(_gameManager.ObstacleVelocity);
                newObstacle.transform.SetParent(_playPan.transform);
            }

            GameObject newScoreBlock =Instantiate(_scoreBlock, new Vector3(14.0f,0.0f,10.0f), transform.rotation) as GameObject;
            newScoreBlock.GetComponent<ScoreBlock>().Initialize(_gameManager.ObstacleVelocity);
            newScoreBlock.transform.SetParent(_playPan.transform);

            _time=0.0f;
        }
        else if(_time>5.0f) 
        {
            Destroy(_playPan);
            _titleTr.gameObject.SetActive(true);
            _score.gameObject.SetActive(false);
        }
    }
}
