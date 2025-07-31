using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgrateSystem : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private ReactorSystem _reactor;

    [SerializeField] private int _price = 100;
    [SerializeField] private int _inflation = 10;

    [SerializeField] private TextMeshProUGUI[] _texts;

    public int Price => _price;

    public void UpgratePlayerSpeed()
    {
        if (_reactor.GetCurrentScore() >= _price)
        {
            _player.UpgradePlayerSpeed();
            _reactor.PuyThisScore(_price);
            UpdateTextPrice();
        }
    }

    public void UpgratePlayerBag()
    {
        if (_reactor.GetCurrentScore() >= _price)
        {
            _player.UpgradeCarryCapacity();
            _reactor.PuyThisScore(_price);
            UpdateTextPrice();
        }
    }

    public void UpgrateReactorSpeed()
    {
        if (_reactor.GetCurrentScore() >= _price)
        {
            _reactor.UpSpeedHeatingReactor();
            _reactor.PuyThisScore(_price);
            UpdateTextPrice();
        }
    }

    public void UpgrateMonye()
    {
        if (_reactor.GetCurrentScore() >= _price)
        {
            _reactor.IncreaseIncome();
            _reactor.PuyThisScore(_price);
            UpdateTextPrice();
        }
    }

    private void UpdateTextPrice()
    {
        _price += _inflation;

        foreach (var text in _texts)
            text.text = $"Стоимость {_price} очков";
    }
}
