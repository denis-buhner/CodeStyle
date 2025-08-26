using System.Collections;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Bullet _prefab;
    [SerializeField] private float _shootingDelay;
    [SerializeField] private Transform _objectToShoot;
    [SerializeField] private float _spawnOffset;

    private Coroutine _spawn;

    private void OnEnable()
    {
        if(_spawn == null)
        {
            _spawn = StartCoroutine(Spawn());
        }
    }

    private void OnDisable()
    {
        if (_spawn != null)
        {
            StopCoroutine(_spawn);
            _spawn = null;
        }
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_shootingDelay);

        while (isActiveAndEnabled)
        {
            Vector3 direction = (_objectToShoot.position - transform.position).normalized;
            Vector3 spawnPosition = transform.position + direction * _spawnOffset;

            Bullet newBullet = Instantiate(_prefab, spawnPosition, Quaternion.identity);
            newBullet.Initialize(direction, _bulletSpeed);

            yield return waitForSeconds;
        }   
    }
}