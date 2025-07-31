using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;

    [Header("Parameters")]
    [SerializeField]  private string _isRunningParam = "isRun";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetRunning(bool isRunning)
    {
        _animator.SetBool(_isRunningParam, isRunning);
    }
}
