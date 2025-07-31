using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MiniGame : MonoBehaviour, IPointerClickHandler
{
    public List<Image> Lamps;
    public List<Image> Diodes;

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _diodeDelay = 1f;
    [SerializeField] private float _gameDuration = 30f;
    [SerializeField] private GameState _gameState;

    private ReactorSystem _reactorSystem;
    private int _currentDiodeIndex;
    private bool _isGameActive;
    private float _timeRemaining;
    private bool _isProcessingClick;
    private bool _isClockwise = true;
    private Coroutine _diodesCoroutine;

    private void Start()
    {
        _reactorSystem = FindObjectOfType<ReactorSystem>();
        ResetMiniGame();
    }

    private void Update()
    {
        if (_isGameActive)
        {
            UpdateTimer();
            ExitMiniGame();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isGameActive && !_isProcessingClick)
        {
            StartCoroutine(ProcessClick());
        }
    }

    public void StartMiniGame()
    {
        if (_isGameActive) return;

        if (_gameState != null)
        {
            _gameState.SetPlayerControl(false);
            _gameState.SetReactorControl(false);
        }

        gameObject.SetActive(true);
        _isProcessingClick = false;
        _isGameActive = true;
        _timeRemaining = _gameDuration;
        _currentDiodeIndex = 0;
        _isClockwise = true;

        ResetMiniGame();
        _diodesCoroutine = StartCoroutine(ActivateDiodes());
    }

    public void EndMiniGame(bool isWin)
    {
        if (!_isGameActive) return;

        if (_gameState != null)
        {
            _gameState.SetPlayerControl(true);

            if (_reactorSystem != null)
                _gameState.SetReactorControl(true);
        }

        _isGameActive = false;

        if (_diodesCoroutine != null)
        {
            StopCoroutine(_diodesCoroutine);
        }

        if (_reactorSystem != null)
        {
            if (isWin)
            {
                _reactorSystem.SetTemperature(200f);
            }
            else
            {
                _reactorSystem.Explode();
            }
        }

        gameObject.SetActive(false);
    }

    private void ExitMiniGame()
    {
        if (Input.GetKeyDown(KeyCode.F))
            EndMiniGame(false);
    }

    private void UpdateTimer()
    {
        _timeRemaining -= Time.deltaTime;
        _timerText.text = Mathf.CeilToInt(_timeRemaining).ToString();

        if (_timeRemaining <= 0)
        {
            EndMiniGame(false);
        }
    }

    IEnumerator ProcessClick()
    {
        _isProcessingClick = true;

        ToggleLampUnderCurrentDiode();

        _isClockwise = !_isClockwise;

        yield return new WaitForSeconds(0.1f);
        _isProcessingClick = false;
    }

    private void ToggleLampUnderCurrentDiode()
    {
        if (_currentDiodeIndex < 0 || _currentDiodeIndex >= Lamps.Count) return;

        Image lamp = Lamps[_currentDiodeIndex];
        lamp.color = lamp.color == Color.gray ? Color.yellow : Color.gray;

        if (CheckWinCondition())
        {
            EndMiniGame(true);
        }
    }

    IEnumerator ActivateDiodes()
    {
        while (_isGameActive)
        {
            foreach (var diode in Diodes)
            {
                if (diode != null)
                    diode.color = Color.gray;
            }

            if (_currentDiodeIndex >= 0 && _currentDiodeIndex < Diodes.Count && Diodes[_currentDiodeIndex] != null)
            {
                Diodes[_currentDiodeIndex].color = Color.green;
            }

            yield return new WaitForSeconds(_diodeDelay);

            if (_isClockwise)
            {
                _currentDiodeIndex = (_currentDiodeIndex + 1) % Diodes.Count;
            }
            else
            {
                _currentDiodeIndex = (_currentDiodeIndex - 1 + Diodes.Count) % Diodes.Count;
            }
        }
    }

    private void ResetMiniGame()
    {
        foreach (var lamp in Lamps)
        {
            if (lamp != null)
                lamp.color = Color.gray;
        }

        foreach (var diode in Diodes)
        {
            if (diode != null)
                diode.color = Color.gray;
        }
    }

    private bool CheckWinCondition()
    {
        foreach (var lamp in Lamps)
        {
            if (lamp == null || lamp.color != Color.yellow)
            {
                return false;
            }
        }
        return true;
    }
}