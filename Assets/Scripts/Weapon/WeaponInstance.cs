using UnityEngine;

public class WeaponInstance
{
    public int currentLevel;
    public WeaponData data;

    // Resolved values cache
    public float cooldown { get; private set; }
    public float damage { get; private set; }
    public float projSpeed { get; private set; }
    public int pierce { get; private set; }

    void OnEnable()
    {
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.OnStatsChanged += RecalculateDerivedStats;
        RecalculateDerivedStats();
    }

    void OnDisable()
    {
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.OnStatsChanged -= RecalculateDerivedStats;
    }

    public void SetLevel(int lv)
    {
        currentLevel = Mathf.Max(1, lv);
        RecalculateDerivedStats();
    }

    public void RecalculateDerivedStats()
    {
        if (data == null) return;
        int i = Mathf.Clamp(currentLevel - 1, 0, 999);

        // per-level multipliers from WeaponData
        float wRate = GetOrDefault(data.fireRateMultiplierPerLevel, i, 1f);
        float wDmg = GetOrDefault(data.damageMultiplierPerLevel, i, 1f);
        float wSpeed = GetOrDefault(data.speedMultiplierPerLevel, i, 1f);
        float wPierce = GetOrDefault(data.pierceBonusPerLevel, i, 0f);

        // global multipliers from PlayerStats
        var ps = PlayerStats.Instance;
        float gRate = ps != null ? ps.GetGlobalFireRateMul() : 1f;
        float gDmg = ps != null ? ps.GetGlobalDamageMul() : 1f;
        float gCd = ps != null ? ps.GetCooldownMul() : 1f;
        float gSpd = ps != null ? ps.GetProjectileSpeedMul() : 1f;
        float gPr = ps != null ? ps.GetPierceAdd() : 0f;

        // base values (pull from WeaponData / ProjectileData)
        float baseCd = Mathf.Max(0.0001f, data.attackCooldown);
        float baseDmg = GetProjectileBaseDamage(data);
        float baseSpd = GetProjectileBaseSpeed(data);
        int basePr = GetProjectileBasePierce(data);

        // Apply formulas
        float rateMul = Mathf.Max(0.0001f, wRate * gRate);
        cooldown = (baseCd / rateMul) * gCd;
        damage = baseDmg * wDmg * gDmg;
        projSpeed = baseSpd * wSpeed * gSpd;
        pierce = Mathf.Max(0, Mathf.RoundToInt(basePr + wPierce + gPr));
    }

    static float GetOrDefault(float[] arr, int idx, float fallback)
        => (arr != null && idx >= 0 && idx < arr.Length) ? arr[idx] : fallback;

    // These helpers should read your ProjectileData fields directly if possible.
    static float GetProjectileBaseDamage(WeaponData wd)
    {
        // TODO: replace with direct field access in your ProjectileData
        // e.g., return wd.projectileData.baseDamage;
        return 1f;
    }
    static float GetProjectileBaseSpeed(WeaponData wd)
    {
        // TODO: replace with direct field access in your ProjectileData
        // e.g., return wd.projectileData.baseSpeed;
        return 1f;
    }
    static int GetProjectileBasePierce(WeaponData wd)
    {
        // TODO: replace with direct field access in your ProjectileData
        // e.g., return wd.projectileData.basePierce;
        return 0;
    }
}
