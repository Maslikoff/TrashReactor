using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

public class ReactorSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _currentTemperature = 550f;
    [SerializeField] private float _speedHeating = 2f;
    [SerializeField] private float _minTemp = 0f;
    [SerializeField] private float _maxTemp = 1000f;
    [SerializeField] private float _baseCoolingRate = 5f;
    [SerializeField] private float _specialHeatRate = 10f;
    [SerializeField] private Vector2 _optimalRange = new Vector2(350f, 650f);
    [SerializeField] private float _specialThreshold = 555f;
    [SerializeField] private float _colorChangeStart = 200f;
    [SerializeField] private float _colorChangeEnd = 800f;

    [Header("Management")]
    [SerializeField] private float _score = 100f;
    [SerializeField] private float _timeDifference = 20f;
    [SerializeField] private float _scoreMultiplier = 2f;


    [Header("References")]
    [SerializeField] private GameObject _objectReactor;
    [SerializeField] private Slider _temperatureSlider;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private MiniGame _miniGameUI;
    [SerializeField] private LosePanel _losePanel;
    [SerializeField] private Renderer _reactorRenderer;
    [SerializeField] private GameState _gameState;

    [Header("Sounds")]
    [SerializeField] private AudioSource _audioExp;

    [Header("Colors")]
    [SerializeField] private Color _coldColor = Color.blue;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _hotColor = Color.red;

    [Header("Events")]
    public UnityEvent OnReactorExplode;
    public UnityEvent OnMiniGameStart;

    private bool _isExploded = false;
    private float _scoreRate = 10f;
    private Material _reactorMaterial;

    private void Awake()
    {
        YG2.saves.coins = (int)_score;
        YG2.SaveProgress();

        if (_reactorRenderer != null)
            _reactorMaterial = _reactorRenderer.material;
    }

    private void Update()
    {
        YG2.saves.coins = (int)_score;

        if (_isExploded) return;

        UpdateTemperature();
        UpdateScore();
        CheckSpecialConditions();
        UpdateReactorColor();
        UpdateUI();
    }

    public void PuyThisScore(int price)
    {
        _score -= price;
        YG2.SaveProgress();
    }

    public void AddMonye(int addMonye)
    {
        _score += addMonye;
        YG2.SaveProgress();
    }

    public int GetCurrentScore()
    {
        YG2.SaveProgress();
        return (int)_score;
    }

    public void SetTemperature(float temperature)
    {
        _currentTemperature = temperature;

        Debug.Log($"Реактор стабилизирован! Температура: {_currentTemperature}°C");
    }

    public void UpSpeedHeatingReactor()
    {
        if (_speedHeating <= 1)
            _speedHeating -= 0.1f;
    }

    public void IncreaseIncome()
    {
        _timeDifference--;
    }

    public void Explode()
    {
        _isExploded = true;

        if (_explosionEffect != null)
            _explosionEffect.Play();

        OnReactorExplode?.Invoke();

        if (_audioExp != null)
            _audioExp.Play();

        _losePanel.OpenLosePanel();

        GameState.Instance?.EndGame();
       // _gameState.EndGame();

        Destroy(_objectReactor, 2f);

        Debug.Log("Explode");
    }

    private void UpdateTemperature()
    {
        float tempChange = -_baseCoolingRate * _speedHeating * Time.deltaTime;

        if (_currentTemperature > _specialThreshold)
            tempChange += _specialHeatRate * _speedHeating * Time.deltaTime;

        _currentTemperature = Mathf.Clamp(_currentTemperature + tempChange, _minTemp, _maxTemp);
    }

    private void UpdateReactorColor()
    {
        if (_reactorMaterial == null) return;

        if (_currentTemperature <= _colorChangeStart)
        {
            float t = _currentTemperature / _colorChangeStart;
            _reactorMaterial.color = Color.Lerp(_coldColor, _normalColor, t);
        }
        else if (_currentTemperature >= _colorChangeEnd)
        {
            float t = (_currentTemperature - _colorChangeEnd) / (_maxTemp - _colorChangeEnd);
            _reactorMaterial.color = Color.Lerp(_normalColor, _hotColor, t);
        }
        else
        {
            _reactorMaterial.color = _normalColor;
        }
    }

    public void AddHeat(float amount)
    {
        if (_isExploded) return;
        _currentTemperature = Mathf.Clamp(_currentTemperature + amount, _minTemp, _maxTemp);
    }

    public void OnItemThrown(string itemType)
    {
        switch (itemType)
        {
            case "Trash": AddHeat(100f); break;
            case "Dust": AddHeat(-80f); break;
        }
    }

    private void UpdateScore()
    {
        float multiplier = IsInOptimalRange() ? _scoreMultiplier : 1f;
        _score += _scoreRate * multiplier * Time.deltaTime / _timeDifference;
    }

    private void CheckSpecialConditions()
    {
        if (_currentTemperature >= _maxTemp)
            Explode();
        else if (_currentTemperature <= _minTemp)
            StartMiniGame();
    }

    private void UpdateUI()
    {
        if (_temperatureSlider != null)
            _temperatureSlider.value = _currentTemperature / _maxTemp;
    }

    private bool IsInOptimalRange() =>
        _currentTemperature >= _optimalRange.x &&
        _currentTemperature <= _optimalRange.y;

    private void StartMiniGame()
    {
        if (_miniGameUI != null) _miniGameUI.StartMiniGame();
        OnMiniGameStart?.Invoke();
    }
}
