using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{

    private Camera _mainCam;

    [SerializeField]
    private LayerMask _targetLayer;
    [SerializeField]
    private Canvas _weaponSelectCanvas;
    [SerializeField]
    private Button _lvl1Btn;
    [SerializeField]
    private Button _lvl2Btn;
    [SerializeField]
    private Button _lvl3Btn;
    [SerializeField]
    private Button _resetBtn;

    [SerializeField]
    private Transform _weaponRangeVisualizer;
    private WeaponBase _weaponBaseSelected;

    private void Awake() {
        _mainCam = Camera.main;

        _lvl1Btn.onClick.AddListener(delegate { OnWeaponLvlUpdateClick(WeaponLvl.LEVEL_1); });
        _lvl2Btn.onClick.AddListener(delegate { OnWeaponLvlUpdateClick(WeaponLvl.LEVEL_2); });
        _lvl3Btn.onClick.AddListener(delegate { OnWeaponLvlUpdateClick(WeaponLvl.LEVEL_3); });
        _resetBtn.onClick.AddListener(ResetGame);
    }
    private void OnDestroy() {
        _lvl1Btn.onClick.RemoveListener(delegate { OnWeaponLvlUpdateClick(WeaponLvl.LEVEL_1); });
        _lvl2Btn.onClick.RemoveListener(delegate { OnWeaponLvlUpdateClick(WeaponLvl.LEVEL_2); });
        _lvl3Btn.onClick.RemoveListener(delegate { OnWeaponLvlUpdateClick(WeaponLvl.LEVEL_3); });
        _resetBtn.onClick.RemoveListener(ResetGame);
    }
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
/*            if(EventSystem.current.IsPointerOverGameObject())
                return;*/

            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if(Physics.Raycast(ray, out rayHit, Mathf.Infinity, _targetLayer)) {
           //     print("obj clicked");
                    _weaponBaseSelected = rayHit.collider.GetComponent<WeaponBase>();

                if(_weaponBaseSelected.IsWeaponActive) { 
                    PopupHandler.Instance.DisplayWeaponInfo( _weaponBaseSelected.GetWeaponLevel() );
                    _weaponRangeVisualizer.position = _weaponBaseSelected.transform.position;
                    _weaponRangeVisualizer.localScale = Vector3.one * _weaponBaseSelected.WeaponFireRange * 2;
                    _weaponRangeVisualizer.gameObject.SetActive(true);
                }

                _weaponSelectCanvas.transform.parent = rayHit.transform;
                _weaponSelectCanvas.transform.localPosition = new Vector3 (
                    0, 8f, 0.5f
                    );
                
             //   _weaponSelectCanvas.transform.localPosition = Vector3.up * 8f;
                ActivateWeaponSelectCanvas(true);
            } else {
                if(EventSystem.current.IsPointerOverGameObject())
                    return;
                _weaponBaseSelected = null;
                _weaponRangeVisualizer.gameObject.SetActive(false);
             //   print("disabling");
                ActivateWeaponSelectCanvas(false);
                PopupHandler.Instance.ClearWeaponInfo();
            }
        }
    }
    private void ActivateWeaponSelectCanvas(bool active) {
        _weaponSelectCanvas.gameObject.SetActive(active);
    }
    private void OnWeaponLvlUpdateClick(WeaponLvl weaponLvl) {
        if (GoldManager.Instance.CurrentGold < _weaponBaseSelected.GetWeaponCost(weaponLvl)) {
            PopupHandler.Instance.DisplayPopup("Insufficient gold");
            return;
        }
        _weaponBaseSelected.UpdateWeapon(weaponLvl);
        _weaponRangeVisualizer.gameObject.SetActive(false);
        PopupHandler.Instance.DisplayPopup("Purchase success!");
        PopupHandler.Instance.ClearWeaponInfo();
    }
    private void ResetGame() {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;

    }
}
