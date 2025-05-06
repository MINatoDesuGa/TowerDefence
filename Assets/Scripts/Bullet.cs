using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Transform _targetTransform;
    [SerializeField]
    private float _speed = 10f;

    private Weapon _associatedWeapon;
    private Coroutine _chaseCoroutine;
    private void OnEnable() {
        if (_targetTransform == null || CheckNotInRange()) {
            Reset();
            return;
        }
        _chaseCoroutine = StartCoroutine(ChaseTarget());
    }
    private void OnDisable() {
        if (_chaseCoroutine != null)
            StopCoroutine(_chaseCoroutine);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Enemy")) {
            Reset();
            //    EnemyAI.OnEnemyDead?.Invoke();
        }
    }
    //---------------------------------------------------------------//
    public void SetTarget(Transform targetTransform) => _targetTransform = targetTransform;

    private void Reset() {
        gameObject.SetActive(false);
        if(_targetTransform != null) {
            _associatedWeapon.RemoveTarget(_targetTransform);
            _targetTransform = null;
        }
    }
    public void AssignAssociatedWeapon(Weapon weapon) => _associatedWeapon = weapon; 
    private IEnumerator ChaseTarget() {
        while(true) {
            if(CheckNotInRange()) { 
                Reset();
                yield break;
            }
            transform.SetPositionAndRotation (
                Vector3.MoveTowards(transform.position, _targetTransform.position, _speed * Time.deltaTime),
                Quaternion.LookRotation((_targetTransform.position - transform.position).normalized)
            );
            yield return null;
        }
    }
    private bool CheckNotInRange() {
        return Vector3.Distance(transform.position, _targetTransform.position) > _associatedWeapon.GetFireRange() * 2;
    }
}
