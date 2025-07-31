using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private ReactorSystem _reactor;

    private void Update()
    {
        if (_reactor != null && _scoreText != null)
            _scoreText.text = $"Очков переработки: {(int)_reactor.GetCurrentScore()}";
    }
}
