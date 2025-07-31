using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [SerializeField] private Transform weaponHolder;

    private List<Weapon> weapons = new List<Weapon>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Weapon AddWeapon(WeaponData data)
    {
        // 이미 있는 무기라면 리턴하지 말고 새로 생성 (중복 허용 안하면 이 조건 추가)
        if (HasWeapon(data))
            return null;

        GameObject weaponObj = Instantiate(data.weaponPrefab, weaponHolder); // 무기 프리팹 사용
        Weapon weapon = weaponObj.GetComponent<Weapon>();
        weapon.Initialize(data, 1);
        weapons.Add(weapon);
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
            weapon.Upgrade();
        else
            Debug.LogWarning($"업그레이드 실패: 무기 {data.weaponName} 없음");
    }

    public List<Weapon> GetAllWeapons()
    {
        return weapons;
    }
}
