using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public static Action OnWaveEnd;
    public static int CurrentAliveEnemyCount;

    [SerializeField]
    private int _totalWaves;
    [SerializeField]
    private TMPro.TMP_Text _currentWaveText;
    [SerializeField] 
    private List<GameObject> _enemyPool;
    [SerializeField]
    private GameObject _enemyPrefab;

    private WaitForSeconds _spawnDelay = new(GameConfig.EACH_ENEMY_SPAWN_DELAY);
    private int _currentWave = 1;
    private Coroutine _currentWaveCoroutine;
    private int _currentAliveEnemyCount;
    private int _currentMaxSpawnableEnemy;
    private void Awake() {
        EnemyAI.OnEnemyDead +=  UpdateWaveStatus;
    }
    private void OnDestroy() {
        EnemyAI.OnEnemyDead -= UpdateWaveStatus;
    }
    private void Start() {
        UpdateWaveUI();
        _currentWaveCoroutine = StartCoroutine( SpawnEnemy() );
    }
    //----------------------------------------------------------------------//
    private void UpdateWaveUI() {
        _currentWaveText.text = "WAVE " + _currentWave;
    }
    private void UpdateWaveStatus() {
 --CurrentAliveEnemyCount;
        //--_currentAliveEnemyCount;
        if( CurrentAliveEnemyCount != 0 )
            return;
        //all enemies are dead proceed to next wave
        OnWaveEnd?.Invoke();
        ++_currentWave;
        if(_currentWave >  _totalWaves ) {
            GameConfig.OnGameOver?.Invoke();
            PopupHandler.Instance.DisplayGameInfo("CONGRATS, reset to play again");
            Debug.Log("game over");
            return;
        }

        UpdateWaveUI();
        _currentWaveCoroutine = StartCoroutine(SpawnEnemy());
    }
    private IEnumerator SpawnEnemy() {
        var maxSpawnable = _currentWave * GameConfig.ENEMY_COUNT_EACH_WAVE;
        CurrentAliveEnemyCount = maxSpawnable;
        for (int i = 0; i < maxSpawnable; ++i) {
            var enemyObj = GetAvailableEnemyObj();   
            enemyObj.SetActive(true);
            yield return _spawnDelay;
        }
    }
    private GameObject GetAvailableEnemyObj() {
        foreach(var enemy in _enemyPool) { 
            if(!enemy.activeInHierarchy) 
                return enemy;
        }

        var spawnPos = transform.position + (Vector3.right * (UnityEngine.Random.Range(-GameConfig.MAX_SPAWN_xOFFSET, GameConfig.MAX_SPAWN_xOFFSET)));

        var newEnemyObj = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
        newEnemyObj.transform.parent = transform;
        _enemyPool.Add(newEnemyObj);  

        return newEnemyObj;
    }

}
