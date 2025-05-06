using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int _cost;
    [Range(1f, GameConfig.FIRE_DELAY_THRESHOLD)]
    [SerializeField]
    private int _fireRate;
    [Range(2f, GameConfig.MAX_FIRE_RANGE)]
    [SerializeField]
    private float _fireRange;

    private SphereCollider _collider;
    private Coroutine _fireCoroutine;
    private WaitForSeconds _fireDelay;

    private HashSet<Transform> _availableTargets = new();

    private void OnEnable() {
        _collider = GetComponent<SphereCollider>();
        EnemyWave.OnWaveEnd += Reset;
     //   UpdateWeaponProperty(_fireRange, _fireRate);
    }
    private void OnDisable() {
        EnemyWave.OnWaveEnd -= Reset;
    }
    private void OnTriggerEnter(Collider triggerObj) {
        AddTargetInRange(triggerObj.transform);
        if (_fireCoroutine == null) {
            _fireCoroutine = StartCoroutine(FireSequence());
        }
    }
    private void OnTriggerExit(Collider other) {
        RemoveTarget(other.transform);
    }
    //---------------------------------------------//
    private void Reset() {
        _availableTargets.Clear();
        if(_fireCoroutine != null ) { 
            StopCoroutine( _fireCoroutine );
            _fireCoroutine = null;
        }
    }
    public void UpdateWeaponProperty(float fireRange, int fireRate) {
        if(_collider == null) {
            _collider= GetComponent<SphereCollider>();
        }
        _fireRange = _collider.radius = fireRange;
        _fireDelay = new(GameConfig.FIRE_DELAY_THRESHOLD / fireRate);
    }
    public float GetFireRange() {
        return _fireRange;
    } 
    private void AddTargetInRange(Transform target)=> _availableTargets.Add(target);
    public void RemoveTarget(Transform target)=> _availableTargets.Remove(target);
    private IEnumerator FireSequence() {
        while(true) {
            
            Bullet bullet = BulletPool.Instance.GetAvailableBulletObj(transform.position);
            var targetTransform = GetTargetTransform();

            if(targetTransform != null ) {
                transform.rotation = Quaternion.LookRotation((targetTransform.position - transform.position).normalized);
                bullet.SetTarget(targetTransform);
                bullet.AssignAssociatedWeapon(this);
                bullet.gameObject.SetActive(true);
            }
            
            yield return _fireDelay;
        }
    }
    private Transform GetTargetTransform() {
        foreach(Transform targetObj in _availableTargets) { 
            if(targetObj.gameObject.activeInHierarchy) { 
                return targetObj;
            }
         //   RemoveTarget(targetObj);
        }

        return null;
    }
}
