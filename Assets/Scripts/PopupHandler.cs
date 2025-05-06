using System.Collections;
using UnityEngine;

public class PopupHandler : MonoBehaviour
{
    public static PopupHandler Instance { get; private set; }
    [SerializeField]
    private TMPro.TMP_Text _popupText;
    [SerializeField]
    private TMPro.TMP_Text _miscInfo;
    private Coroutine _popupCoroutine;
    private WaitForSeconds _popupDelay;
    private void Awake() {
        if(Instance == null) { 
            Instance = this;
        } else {
            Destroy(this);
        }
        _popupDelay = new(GameConfig.POPUP_DURATION);
    }
    //---------------------------------------------------------------//
    public void DisplayWeaponInfo(int weaponLvl) {
        _miscInfo.text = "LEVEL "+ weaponLvl + " WEAPON";
    }
    public void DisplayGameInfo(string info) {
        _miscInfo.text = info;
    }
    public void ClearWeaponInfo() {
        _miscInfo.text = "";
    }
    public void DisplayPopup(string popupText) {
        if(_popupCoroutine != null) { 
            StopCoroutine(_popupCoroutine);
        }
        _popupText.text = popupText;
        _popupCoroutine = StartCoroutine(DelayDisablePopup());
        
    }
    private IEnumerator DelayDisablePopup() {
        yield return _popupDelay;
        _popupText.text = "";
    }
}
