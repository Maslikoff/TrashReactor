using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpaceshipPathFollower : MonoBehaviour
{
    [Header("Path Settings")]
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private float _pauseTime = 5f;
    [SerializeField] private float _arrivalThreshold = 0.5f;
    [SerializeField] private bool _loopPath = true;

    private int _currentWaypointIndex = 0;
    private bool _isPaused = false;
    private Vector3 pausePosition;

    private void Start()
    {
        if (_waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned!");
            enabled = false;
            return;
        }

        StartCoroutine(PauseAtWaypoint());
    }

    private void Update()
    {
        if (_isPaused || _waypoints.Count == 0) return;

        Transform target = _waypoints[_currentWaypointIndex];

        RotateTowards(target.position);

        transform.position = Vector3.MoveTowards(transform.position, target.position, _moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= _arrivalThreshold)
        {
            if (_currentWaypointIndex == 0)
                StartCoroutine(PauseAtWaypoint());
            else
                GoToNextWaypoint();
        }
    }

    private IEnumerator PauseAtWaypoint()
    {
        _isPaused = true;
        pausePosition = transform.position;

        yield return new WaitForSeconds(_pauseTime);

        _isPaused = false;
        GoToNextWaypoint();
    }

    private void GoToNextWaypoint()
    {
        _currentWaypointIndex++;

        if (_currentWaypointIndex >= _waypoints.Count)
        {
            if (_loopPath)
                _currentWaypointIndex = 0;
            else
                enabled = false;
        }
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _rotationSpeed * Time.deltaTime);
        }
    }
}