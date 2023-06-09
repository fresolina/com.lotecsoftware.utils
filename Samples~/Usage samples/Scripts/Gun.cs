using Lotec.Utils;
using UnityEngine;
using UnityEngine.Assertions;

public class Gun : MonoBehaviour {
    [SerializeField] float _launchForce = 1000f;
    [SerializeField] int _bulletsPerClick = 3;
    [Tooltip("Use PoolManager")]
    [SerializeField] bool _poolObjects;
    [Tooltip("Destroy bullet after this many seconds")]
    [SerializeField] float _maxDuration = 2f;
    [SerializeField] GameObject _bulletPrefab;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Fire(_bulletsPerClick);
        }
    }

    void Fire(int count) {
        for (int i = 0; i < count; i++) {
            Fire();
        }
    }
    void Fire() {
        GameObject bullet;
        if (_poolObjects) {
            bullet = PoolManager.Instantiate(_bulletPrefab, transform.position, transform.rotation);
        } else {
            bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
        }

        bullet.GetComponent<Bullet>().Fire(_launchForce);

        if (_poolObjects) {
            PoolManager.Destroy(bullet, _maxDuration);
        } else {
            Destroy(bullet, _maxDuration);
        }
    }

    void OnValidate() {
        Assert.IsNotNull(_bulletPrefab, "Assign a bullet prefab");
    }
}

