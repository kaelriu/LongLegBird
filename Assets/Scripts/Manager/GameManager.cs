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

    public void Initialize()
    {
        DontDestroyOnLoad(this);
        _mainCamera=GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _gameScore=0;
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

    public bool NowGame(){return _startGame;}
    public void StartGame()
    {
        _gameScore=0;
        _startGame=true;
    }

    public void FinishGame(){_startGame=false;}

    public void PauseGame(bool Pause){_pauseGame=Pause;}
#endregion

    public Vector2 ObstacleVelocity=new Vector2(-8.0f, 0.0f);
}
