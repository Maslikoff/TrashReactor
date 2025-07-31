using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMove _movement;
    [SerializeField] private ItemInteractionSystem _interactionSystem;

    private void Awake()
    {
        if (_movement == null) _movement = GetComponent<PlayerMove>();
        if (_interactionSystem == null) _interactionSystem = GetComponent<ItemInteractionSystem>();
    }

    public void UpgradePlayerSpeed()
    {
        _movement.UpPlayerSpeed();
    }

    public void UpgradeCarryCapacity()
    {
        _interactionSystem.UpgradeCapacity();
    }
}
