using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private SceneName _restartSceneName;
    [SerializeField] private GameState _gameState;

    public void OpenLosePanel()
    {
        if (_gameState != null)
        {
            _gameState.SetPlayerControl(false);
            _gameState.SetReactorControl(false);
        }

        gameObject.SetActive(true);
    }

    public void CloseLosePanel()
    {
        gameObject.SetActive(false);

        if (_gameState != null)
        {
            _gameState.SetPlayerControl(true);
            _gameState.LoadScene(_restartSceneName);
        }
    }
}
