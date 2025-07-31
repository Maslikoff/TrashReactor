using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    GameScene,
    MenuScene
}

public class GameState : MonoBehaviour
{
    [SerializeField] private TimerDisplay _timerDisplay;
    [SerializeField] private Leaderboard _leaderboard;

    private float _currentTime = 0f;

    private bool _playerControlEnabled = true;
    private bool _reactorControlEnabled = true;
    private bool _isGameRunning = true;
    private bool _isPaused = false;

    public static GameState Instance { get; private set; }

    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    private void Update()
    {
        if (_isGameRunning && _isPaused == false)
        {
            _currentTime += Time.deltaTime;
            _timerDisplay.UpdateTimeDisplay(_currentTime);
        }
    }

    public void SetPlayerControl(bool enabled)
    {
        _playerControlEnabled = enabled;
        PauseGame(!enabled);

        FindObjectOfType<PlayerMove>().enabled = _playerControlEnabled;
    }
    
    public void SetReactorControl(bool enabled)
    {
        _reactorControlEnabled = enabled;
        PauseGame(!enabled);

        FindObjectOfType<ReactorSystem>().enabled = _reactorControlEnabled;
    }

    public void LoadScene(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }

    public void EndGame()
    {
        _isGameRunning = false;

        _leaderboard.SubmitTimeToLeaderboard(_currentTime);

        PlayerPrefs.SetFloat("BestTime", _currentTime);
    }

    private void PauseGame(bool enable)
    {
        _isPaused = enable;
    }
}
