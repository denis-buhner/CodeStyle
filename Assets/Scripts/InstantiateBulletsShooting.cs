using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InstantiateBulletsShooting : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Bullet _prefab;
    [SerializeField] private float _timeWaitShooting;
    [SerializeField] private Transform _objectToShoot;
    [SerializeField] private Vector3 _spawnOffset;

    private Coroutine _shooting;

    private void OnEnable()
    {
        if(_shooting == null)
        {
            _shooting = StartCoroutine(Shooting());
        }
    }

    private void OnDisable()
    {
        if (_shooting != null)
        {
            StopCoroutine(_shooting);
            _shooting = null;
        }
    }

    IEnumerator Shooting()
    {
        while (isActiveAndEnabled)
        {
            Vector3 direction = (_objectToShoot.position - transform.position).normalized;

            Bullet newBullet = Instantiate(_prefab, transform.position + _spawnOffset, Quaternion.identity);
            newBullet.Initialize(direction, _bulletSpeed);

            yield return new WaitForSeconds(_timeWaitShooting);
        }   
    }
}