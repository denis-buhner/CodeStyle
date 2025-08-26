using System.Collections;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] private Transform _wayPoints;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed; 
    [SerializeField] private float _moveDelay;
    [SerializeField] private float _stoppingDistance;

    private Transform[] _targets;
    private Coroutine _move;

    private void OnEnable()
    {
        if (TryGetPlaces())
        {
            if(_move == null)
            {
                _move = StartCoroutine(Move());
            }
        }
    }

    private void OnDisable()
    {
        if (_move != null)
        {
            StopCoroutine(_move);
            _move = null;
        }
    }

    [ContextMenu("Refresh Waypoints")]
    private bool TryGetPlaces()
    {
        if (_wayPoints == null || _wayPoints.childCount == 0)
            return false;

        _targets = new Transform[_wayPoints.childCount];

        for (int i = 0; i < _targets.Length; i++)
            _targets[i] = _wayPoints.GetChild(i);

        return _targets.Length > 0;
    }

    private bool IsCloseEnough(Vector3 currentPosition, Vector3 currentTarget)
    {
        return (currentTarget - currentPosition).sqrMagnitude <= _stoppingDistance * _stoppingDistance;
    }

    private IEnumerator Move()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_moveDelay);
        int currentPlaceIndex = 0;

        while (isActiveAndEnabled)
        {
            Vector3 target = _targets[currentPlaceIndex].position;

            while (!IsCloseEnough(transform.position, target))
            {
                transform.position = Vector3.MoveTowards(transform.position, target, _movementSpeed * Time.deltaTime);

                Vector3 direction = (target - transform.position).normalized;

                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
                }

                yield return null;
            }

            currentPlaceIndex = ++currentPlaceIndex % _targets.Length;

            yield return waitForSeconds;  
        }
    }
}