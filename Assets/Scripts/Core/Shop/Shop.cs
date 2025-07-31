using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _interactionHint; 
    [SerializeField] private GameObject _shopUI;

    [Header("Settings")]
    [SerializeField] private float _interactionDistance = 3f;

    private Transform _player;
    private bool _isInRange;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _interactionHint.SetActive(false);
        _shopUI.SetActive(false);
    }

    private void Update()
    {
        CheckPlayerDistance();

        if (_isInRange)
        {
            HandleInput();
        }
        else if (_shopUI.activeSelf)
        {
            CloseShop();
        }
    }

    private void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, _player.position);
        bool wasInRange = _isInRange;
        _isInRange = distance <= _interactionDistance;

        if (_isInRange != wasInRange)
        {
            _interactionHint.SetActive(_isInRange);

            if (!_isInRange)
            {
                CloseShop();
            }
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!_shopUI.activeSelf)
            {
                OpenShop();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && _shopUI.activeSelf)
        {
            CloseShop();
        }
    }

    private void OpenShop()
    {
        _shopUI.SetActive(true);
        _interactionHint.SetActive(false);

        GameState.Instance?.SetPlayerControl(false);
        GameState.Instance?.SetReactorControl(false);
        Time.timeScale = 0f;
    }

    private void CloseShop()
    {
        _shopUI.SetActive(false);

        if (_isInRange)
        {
            _interactionHint.SetActive(true);
        }

        GameState.Instance?.SetPlayerControl(true); 
        GameState.Instance?.SetReactorControl(true);
        Time.timeScale = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _interactionDistance);
    }
}
