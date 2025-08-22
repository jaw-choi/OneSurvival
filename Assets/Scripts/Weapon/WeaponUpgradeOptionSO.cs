using UnityEngine;

[CreateAssetMenu(fileName = "WeaponOption", menuName = "Roguelike/Upgrade Option/Weapon")]
public class WeaponUpgradeOptionSO : UpgradeOptionSO
{
    public WeaponData weaponData;

    private void OnValidate()
    {
        kind = UpgradeKind.Weapon;
        if (string.IsNullOrEmpty(id))
            id = $"weapon_{weaponData?.weaponName}";
        if (string.IsNullOrEmpty(displayName) && weaponData != null)
            displayName = weaponData.weaponName;
        if (icon == null && weaponData != null)
            icon = weaponData.weaponIcon;
    }

    public override bool CanRoll()
    {
        if (WeaponManager.Instance == null || weaponData == null) return false;

        // If you already have the weapon -> check its currentLevel against maxLevel
        if (WeaponManager.Instance.HasWeapon(weaponData))
        {
            int cur = WeaponManager.Instance.GetWeaponLevel(weaponData); // <- 아래 NOTE 참고
            return cur < maxLevel;
        }
        // Not owned yet -> can roll as long as level 0 < max
        return maxLevel > 0;
    }


    public override int GetCurrentLevel()
    {
        if (WeaponManager.Instance == null || weaponData == null) return 0;
        return WeaponManager.Instance.HasWeapon(weaponData)
            ? WeaponManager.Instance.GetWeaponLevel(weaponData)
            : 0;
    }

    public override void Apply()
    {
        if (WeaponManager.Instance == null || weaponData == null) return;

        if (WeaponManager.Instance.HasWeapon(weaponData))
            WeaponManager.Instance.UpgradeWeapon(weaponData);
        else
            WeaponManager.Instance.AddWeapon(weaponData);
    }
}
