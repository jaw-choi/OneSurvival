using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData { get; private set; }

    private IWeaponFireBehaviour fireBehaviour;
    public int currentLevel;
    private float lastFireTime;
    public float TotalDealtDamage { get; private set; } = 0f;
    public float TimeAcquired { get; private set; } = 0f;
    public void Initialize(WeaponData data, int startLevel) 
    {
        weaponData = data;
        currentLevel = Mathf.Max(1, startLevel);
        data.currentLevel = currentLevel;
        SetFireBehaviourByType(weaponData.fireType);
        lastFireTime = -999f;
        TimeAcquired = Time.time;
        TotalDealtDamage = 0f;
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
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);

            lastFireTime = Time.time;
        }
    }
    public void AddDamage(float amount)
    {
        TotalDealtDamage += amount;
    }
    public void Upgrade()
    {
        currentLevel++;
        weaponData.currentLevel = currentLevel;
        Debug.Log($"{weaponData.weaponName} ���� ������ �� Lv.{currentLevel}");
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
        float baseMult = GetValueOrDefault(weaponData.damageMultiplierPerLevel, 1f);
        float permBonus = 1f + PermanentStatManager.GetValue(PermanentStatType.Damage);
        return baseMult * permBonus;
    }

    public float GetFireRateMultiplier()
    {
        return GetValueOrDefault(weaponData.fireRateMultiplierPerLevel, 1f);
    }

    public float GetSpeedMultiplier()
    {
        float baseMult = GetValueOrDefault(weaponData.speedMultiplierPerLevel, 1f);
        float permBonus = 1f + PermanentStatManager.GetValue(PermanentStatType.ProjectileSpeed);
        return baseMult * permBonus;
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
