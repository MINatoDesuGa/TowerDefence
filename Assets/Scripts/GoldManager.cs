using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    public int CurrentGold { get; private set;} = 10;
    [SerializeField]
    private TMPro.TMP_Text _goldText;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
        EnemyAI.OnEnemyDead += IncreaseGold;
    }
    private void OnDestroy() {
        EnemyAI.OnEnemyDead -= IncreaseGold;
    }
    private void Start() {
        UpdateGoldUI();
    }
    //----------------------------------------------------//
    private void IncreaseGold() {
        CurrentGold += GameConfig.ENEMY_GOLD_VALUE;
        UpdateGoldUI();
    }
    public void DecreaseGold(int goldCost) {
        CurrentGold -= goldCost;
        UpdateGoldUI();
    }
    private void UpdateGoldUI() {
        _goldText.text = "GOLD : " + CurrentGold;
    }
}
