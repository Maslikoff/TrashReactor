using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _smoothSpeed = 0.125f;
    [SerializeField] private Vector3 _offset;

    private void LateUpdate()
    {
        Vector3 diseredPosition = _player.position + _offset;

        Vector3 smoothPosition = Vector3.Lerp(transform.position, diseredPosition, _smoothSpeed);

        transform.position = smoothPosition;

        transform.LookAt(_player);
    }
}
