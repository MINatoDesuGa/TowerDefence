using System;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour {
    public static Action OnEnemyDead;

    [SerializeField]
    private int _hp;
    [Range(1, 5)]
    [SerializeField]
    private float _moveSpeed = 2;

    [SerializeField]
    private Transform _targetTransform;

    private NavMeshAgent _navAgent;
    private Vector3 _defaultPosition;
    private void Awake() {
        _navAgent = GetComponent<NavMeshAgent>();
    }
    private void Start() {
       
        _defaultPosition = transform.position;
        _navAgent.speed = _moveSpeed;
    }
    private void OnEnable() {
        if (_targetTransform == null) {
            _targetTransform = House.Transform;
        }
        MoveTowardsTarget();
    }
    private void OnCollisionEnter(Collision collidedObj) {
        switch(collidedObj.collider.tag) {
            case "Bullet":
                OnEnemyDead?.Invoke();
                Reset();
                break;
            case "House":
                EnemyWave.CurrentAliveEnemyCount--;
               
                Reset();
                break;
        }
        
    }
    //----------------------------------------------------------//
    private void MoveTowardsTarget() { 
        _navAgent.SetDestination(_targetTransform.position);
    }
    private void Reset() {
        gameObject.SetActive(false);
        transform.position = _defaultPosition;
    }

}
