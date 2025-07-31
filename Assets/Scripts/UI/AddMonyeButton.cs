using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;

public class AddMonyeButton : MonoBehaviour
{
    public string rewardID;

    [SerializeField] private ReactorSystem _reactorSystem;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private float _showInterval = 120f;
    [SerializeField] private int _addMonye;

    private bool _isRunning = true;

    private void Start()
    {
        _button.gameObject.SetActive(false);
        _textMeshPro.text = $"+{_addMonye} очков";

        StartCoroutine(ShowButtonPeriodically());
    }

    public void OnButtonClicked()
    {
        YG2.RewardedAdvShow(rewardID, () =>
        {
            _button.gameObject.SetActive(false);
            _reactorSystem.AddMonye(_addMonye);
            _reactorSystem.SetTemperature(500);
        });
    }

    private IEnumerator ShowButtonPeriodically()
    {
        while (_isRunning)
        {
            yield return new WaitForSeconds(_showInterval);

            _button.gameObject.SetActive(true);

            yield return new WaitUntil(() => !_button.gameObject.activeSelf);
        }
    }
}
