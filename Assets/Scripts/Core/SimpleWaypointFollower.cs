using UnityEngine;
using System.Collections;

public class SimpleWaypointFollower : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _pauseTime = 5f;

    private int _currentIndex = 0;
    private bool _isPaused = false;

    private void Start()
    {
        if (_waypoints == null || _waypoints.Length == 0)
        {
            Debug.LogError("Waypoints not assigned!");
            return;
        }

        transform.position = _waypoints[0].position;
        StartCoroutine(MoveBetweenWaypoints());
    }

    private IEnumerator MoveBetweenWaypoints()
    {
        while (true)
        {
            if (_currentIndex == 0)
            {
                _isPaused = true;
                yield return new WaitForSeconds(_pauseTime);
                _isPaused = false;
            }

            while (Vector3.Distance(transform.position, _waypoints[_currentIndex].position) > 0.1f)
            {
                if (!_isPaused) 
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        _waypoints[_currentIndex].position,
                        _speed * Time.deltaTime
                    );

                    Vector3 direction = (_waypoints[_currentIndex].position - transform.position).normalized;
                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            targetRotation,
                            _speed * Time.deltaTime
                        );
                    }
                }
                yield return null;
            }

            _currentIndex = (_currentIndex + 1) % _waypoints.Length;
        }
    }
}