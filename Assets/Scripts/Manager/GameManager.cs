using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
                instance.name = "(singleton)GameManager";
            }
            return instance;
        }
    }

    private GameCanvas _gameCanvas;
    public void Initialize(GameCanvas gameCanvas)
    {
        _gameCanvas=gameCanvas;

        DontDestroyOnLoad(this);
        _mainCamera=GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _gameScore=0;
        _highScore=0;
    }

#region GAME VARIABLES
    private Camera _mainCamera;
    
    private int _gameScore;
    public void AddScore()
    {
        if(_startGame) _gameScore++;
    }

    public int ReturnScore()
    {
        return _gameScore;
    }

    public Camera ReturnCam()
    {
        return _mainCamera;
    }

    private bool _startGame=false;
    private bool _pauseGame;
    private int _highScore=0;

    public bool NowGame(){return _startGame;}
    public void StartGame()
    {
        _gameScore=0;
        _startGame=true;
    }

    public void FinishGame()
    {
        _startGame=false;
        if(_gameScore>_highScore) _highScore=_gameScore;
        _gameCanvas.ShowScorePage(_highScore);
    }

    public void PauseGame(bool Pause){_pauseGame=Pause;}

    public int _difficulty=0;

#endregion
}
