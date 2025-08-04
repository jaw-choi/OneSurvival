using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private GameObject aoeVisualizerPrefab;

    private List<Weapon> weapons = new List<Weapon>();
    private GarlicWeapon GarlicWeapons;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Weapon AddWeapon(WeaponData data)
    {
        if (HasWeapon(data))
            return null;

        GameObject weaponObj = Instantiate(data.weaponPrefab, weaponHolder);
        Weapon weapon = weaponObj.GetComponent<Weapon>();
        weapon.Initialize(data, 1);
        weapons.Add(weapon);

        //CheckAoeWeaponStatus();
        return weapon;
    }

    public bool HasWeapon(WeaponData data)
    {
        return weapons.Exists(w => w.weaponData == data);
    }

    public void UpgradeWeapon(WeaponData data)
    {
        Weapon weapon = weapons.Find(w => w.weaponData == data);
        if (weapon != null)
        {
            weapon.Upgrade();
            //CheckAoeWeaponStatus();
        }
        else
        {
            Debug.LogWarning($"업그레이드 실패: 무기 {data.weaponName} 없음");
        }
    }

    public List<Weapon> GetAllWeapons()
    {
        return weapons;
    }

}
