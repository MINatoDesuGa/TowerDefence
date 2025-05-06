using UnityEngine;

public class WeaponBase : MonoBehaviour {
    public bool IsWeaponActive;

    [SerializeField]
    private Weapon _baseWeapon;
    [SerializeField]
    private WeaponLvl _weaponLvl;

    public float WeaponFireRange;
    private void Start() {
        if(!IsWeaponActive) 
            return;

      //  _baseWeapon.gameObject.SetActive(true);
        UpdateWeapon(_weaponLvl);
    }

    public void UpdateWeapon(WeaponLvl weaponLvl) {
        _weaponLvl = weaponLvl;
        float fireRange = GameConfig.MAX_FIRE_RANGE;
        int fireRate = (int)GameConfig.FIRE_DELAY_THRESHOLD;
        int goldCost = GetWeaponCost(weaponLvl);
        switch(weaponLvl) { 
            case WeaponLvl.LEVEL_1:
                fireRange /= 1.5f;
           //     fireRate -= 2;
                break;
            case WeaponLvl.LEVEL_2:
                fireRange /= 1.25f;
                fireRate++;
                break;
            case WeaponLvl.LEVEL_3:
                fireRate += 2;
                break;
        }
        GoldManager.Instance.DecreaseGold(goldCost);
        _baseWeapon.UpdateWeaponProperty(fireRange, fireRate);
        WeaponFireRange = fireRange;
        _baseWeapon.gameObject.SetActive(true);
        IsWeaponActive = true;
    }
    public int GetWeaponLevel() {
        switch(_weaponLvl) { 
            case WeaponLvl.LEVEL_1:
                return 1;
            case WeaponLvl.LEVEL_2:
                return 2;
        }
        return 3;
    }
    public int GetWeaponCost(WeaponLvl weaponLvl) {
        switch (weaponLvl) {
            case WeaponLvl.LEVEL_1:
                return GameConfig.BASE_WEAPON_GOLD_COST;
            case WeaponLvl.LEVEL_2:
                return GameConfig.BASE_WEAPON_GOLD_COST * 2;
        }
        return GameConfig.BASE_WEAPON_GOLD_COST * 3;
    }
}

public enum WeaponLvl {
    LEVEL_1, LEVEL_2, LEVEL_3
}