using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private List<Bullet> _bulletPool;
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    public Bullet GetAvailableBulletObj(Vector3 spawnPos) {
        foreach (var bullet in _bulletPool) {
            if (!bullet.gameObject.activeInHierarchy) {
                bullet.transform.position = spawnPos;
                return bullet;
            }
                
        }

        var newBulletObj = Instantiate(_bulletPrefab, spawnPos, Quaternion.Euler(Vector3.right * 90));
        newBulletObj.transform.parent = transform;
        newBulletObj.SetActive(false);
        var bulletComp = newBulletObj.GetComponent<Bullet>();
        _bulletPool.Add(bulletComp);

        return bulletComp;
    }
}
