using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData { get; private set; }

    private IWeaponFireBehaviour fireBehaviour;
    private int currentLevel;
    private float lastFireTime;

    public void Initialize(WeaponData data, int startLevel)
    {
        weaponData = data;
        currentLevel = startLevel;
        SetFireBehaviourByType(weaponData.fireType);
        lastFireTime = -999f; // 처음엔 즉시 발사 가능하도록
    }

    public bool CanFire()
    {
        float cooldown = weaponData.attackCooldown / GetFireRateMultiplier();
        return Time.time >= lastFireTime + cooldown;
    }

    public void Fire(Vector2 direction)
    {
        if (fireBehaviour != null && CanFire())
        {
            fireBehaviour.Fire(transform.position, direction, weaponData);
            lastFireTime = Time.time;
        }
    }

    public void Upgrade()
    {
        currentLevel++;
        Debug.Log($"{weaponData.weaponName} 무기 레벨업 → Lv.{currentLevel}");
    }

    private void SetFireBehaviourByType(WeaponFireType fireType)
    {
        switch (fireType)
        {
            case WeaponFireType.Burst:
                fireBehaviour = new BurstFireBehaviour(this);
                break;
            //case WeaponFireType.Single:
            //    fireBehaviour = new SingleShotFireBehaviour(this);
            //    break;
            //case WeaponFireType.Area:
            //    fireBehaviour = new AreaFireBehaviour(this);
            //    break;
            //// 필요한 타입 계속 추가
            default:
                fireBehaviour = new BurstFireBehaviour(this);
                break;
        }
    }

    public float GetDamageMultiplier()
    {
        if (weaponData.damageMultiplierPerLevel.Length >= currentLevel)
            return weaponData.damageMultiplierPerLevel[currentLevel - 1];
        else
            return 1f;
    }

    public float GetFireRateMultiplier()
    {
        if (weaponData.fireRateMultiplierPerLevel.Length >= currentLevel)
            return weaponData.fireRateMultiplierPerLevel[currentLevel - 1];
        else
            return 1f;
    }

    public float GetSpeedMultiplier()
    {
        if (weaponData.speedMultiplierPerLevel.Length >= currentLevel)
            return weaponData.speedMultiplierPerLevel[currentLevel - 1];
        else
            return 1f;
    }

    public float GetPierceBonus()
    {
        if (weaponData.pierceBonusPerLevel.Length >= currentLevel)
            return weaponData.pierceBonusPerLevel[currentLevel - 1];
        else
            return 0f;
    }
}
