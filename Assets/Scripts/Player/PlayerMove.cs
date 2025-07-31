using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] float _rotationSpeed = 10f;
    [SerializeField] private PlayerAnimation _playerAnimation;

    private Rigidbody _rigidbody;
    private Vector3 _movementInput;
    private Vector3 _currentVelocity;
    private bool _isMoving;

    public Vector3 MovementInput => _movementInput;
    public Vector3 CurrentVelocity => _currentVelocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _isMoving = _movementInput.magnitude > 0.01f;

        Move();
        Rotate();

        _currentVelocity = _rigidbody.velocity;
    }

    public void UpPlayerSpeed()
    {
        _moveSpeed += 0.5f;
    }

    private void Move()
    {
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.z = Input.GetAxisRaw("Vertical");

        _rigidbody.MovePosition(_rigidbody.position + _movementInput.normalized * _moveSpeed * Time.fixedDeltaTime);

        _playerAnimation.SetRunning(_isMoving);
    }

    private void Rotate()
    {
        if(_movementInput != Vector3.zero)
        {
            Quaternion toRatetion = Quaternion.LookRotation(_movementInput, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRatetion, _rotationSpeed * Time.deltaTime);
        }
    }
}
