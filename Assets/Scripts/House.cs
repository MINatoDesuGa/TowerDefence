using UnityEngine;

public class House : MonoBehaviour
{
    public static System.Action OnHouseDestroyed;
    public static Transform Transform;

    [SerializeField]
    private int _hp;
    [SerializeField]
    private TMPro.TMP_Text _hpText;

    private int _defaultHp;

    private void Start() {
        _defaultHp = _hp;
        Transform = transform;
        UpdateHpUI();
    }
    private void OnCollisionEnter(Collision collidedObj) {
        if(collidedObj.collider.CompareTag("Enemy")) {
            UpdateHp();
        }        
    }

    //---------------------------------------------------------------//

    private void UpdateHp() {
        _hp = Mathf.Clamp(--_hp, 0, _defaultHp); 
        UpdateHpUI();
        
        if(_hp is 0) {
            OnHouseDestroyed?.Invoke();
            PopupHandler.Instance.DisplayGameInfo("GAME OVER, Reset to play again");
            Time.timeScale = 0;
        }
    }
    private void UpdateHpUI() {
        _hpText.text = "<b>HP: </b>" + _hp;
    }
}
