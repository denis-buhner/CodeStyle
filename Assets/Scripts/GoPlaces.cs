using System.Collections;
using UnityEngine;

public class GoPlaces : MonoBehaviour
{
    [SerializeField] private Transform _wayPoints;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed; 
    [SerializeField] private float _moveDelay;
    [SerializeField] private float _stoppingDistance;

    private Transform[] _targets;
    private Coroutine _movement;

    private void OnEnable()
    {
        if (TryGetPlaces())
        {
            if(_movement == null)
            {
                _movement = StartCoroutine(Move());
            }
        }
    }

    private void OnDisable()
    {
        if (_movement != null)
        {
            StopCoroutine(_movement);
            _movement = null;
        }
    }

    private bool TryGetPlaces()
    {
        if (_wayPoints == null || _wayPoints.childCount == 0)
            return false;

        _targets = new Transform[_wayPoints.childCount];

        for (int i = 0; i < _wayPoints.childCount; i++)
            _targets[i] = _wayPoints.GetChild(i);

        if (_targets.Length > 0)
        {
            return true;
        }

        return false;
    }

    private IEnumerator Move()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_moveDelay);
        int currentPlaceIndex = 0;

        while (isActiveAndEnabled)
        {
            Vector3 target = _targets[currentPlaceIndex].position;

            while (IsFarEnough(transform.position, target))
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

            currentPlaceIndex++;

            if(currentPlaceIndex >= _targets.Length)
            {
                currentPlaceIndex = 0;
            }

            yield return waitForSeconds;  
        }
    }

    private bool IsFarEnough(Vector3 currentPosition,Vector3 currentTarget)
    {
        return (currentTarget - currentPosition).sqrMagnitude > _stoppingDistance * _stoppingDistance;
    }
}