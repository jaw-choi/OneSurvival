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
        currentLevel = Mathf.Max(1, startLevel);
        SetFireBehaviourByType(weaponData.fireType);
        lastFireTime = -999f;
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

            case WeaponFireType.Single:
                fireBehaviour = new SingleShotBehaviour(this);
                break;

            case WeaponFireType.Flamethrower:
                fireBehaviour = new FlamethrowerBehaviour(this);
                break;
            case WeaponFireType.Aoe:
                fireBehaviour = new AoeFireBehaviour(this);
                break;
            default:
                Debug.LogWarning($"Unknown FireType: {fireType}, defaulting to Burst");
                fireBehaviour = new BurstFireBehaviour(this);
                break;
        }
    }

    private int LevelIndex => Mathf.Clamp(currentLevel - 1, 0, 5);

    public float GetDamageMultiplier()
    {
        return GetValueOrDefault(weaponData.damageMultiplierPerLevel, 1f);
    }

    public float GetFireRateMultiplier()
    {
        return GetValueOrDefault(weaponData.fireRateMultiplierPerLevel, 1f);
    }

    public float GetSpeedMultiplier()
    {
        return GetValueOrDefault(weaponData.speedMultiplierPerLevel, 1f);
    }

    public float GetPierceBonus()
    {
        return GetValueOrDefault(weaponData.pierceBonusPerLevel, 0f);
    }

    private float GetValueOrDefault(float[] array, float fallback)
    {
        return (array != null && LevelIndex < array.Length) ? array[LevelIndex] : fallback;
    }
}
