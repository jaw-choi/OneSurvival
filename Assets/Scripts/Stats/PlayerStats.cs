using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    [Header("Player")]
    public string characterName = "One Man";

    [Header("Base Stats")]
    public float baseDamage = 5f;
    public float baseMoveSpeed = 3f;
    public float baseMaxHealth = 100f;
    public float baseRegen = 0f;

    [Header("Permanent (read-only multipliers from meta)")]
    // These are derived from PermanentStatManager on each recalc
    private float permDamageMul = 1f;
    private float permMoveSpeedMul = 1f;
    private float permMaxHpMul = 1f;
    private float permRegenAdd = 0f;

    [Header("Runtime Upgrades (in-run only)")]
    // Global combat scalars used by weapons/projectiles
    public float rtGlobalDamageMul = 1f;     // multiply all weapon damages
    public float rtGlobalFireRateMul = 1f;   // multiply all fire rates
    public float rtCooldownMul = 1f;         // multiply final cooldown (lower is faster)
    public float rtProjectileSpeedMul = 1f;  // multiply projectile speed
    public float rtPierceAdd = 0f;           // add pierce count

    // Player core stats
    public float rtMoveSpeedMul = 1f;
    public float rtMoveSpeedAdd = 0f;
    public float rtMaxHpAdd = 0f;
    public float rtRegenAdd = 0f;
    public float rtPickupRangeAdd = 0f;

    [Header("Calculated Stats")]
    public float TotalDamage { get; private set; }
    public float TotalMoveSpeed { get; private set; }
    public float TotalMaxHealth { get; private set; }
    public float TotalRegen { get; private set; }

    public event Action OnStatsChanged;

    void Awake()
    {
        Instance = this;
        RecalculateAll(); // includes permanent + runtime
    }

    // --- Permanent (meta) ---
    private void PullPermanent()
    {
        permDamageMul = 1f + PermanentStatManager.GetValue(PermanentStatType.Damage);
        permMoveSpeedMul = 1f + PermanentStatManager.GetValue(PermanentStatType.MoveSpeed);
        permMaxHpMul = 1f + PermanentStatManager.GetValue(PermanentStatType.MaxHP);
        permRegenAdd = PermanentStatManager.GetValue(PermanentStatType.Regen);
    }

    public void ApplyPermanentUpgrades()
    {
        // kept for backward compatibility
        RecalculateAll();
    }
    // --- Public helpers for meta bonuses ---
    public float GetExpBonusMultiplier()
    {
        return 1f + PermanentStatManager.GetValue(PermanentStatType.ExpBonus);
    }
    public float GetGoldBonusMultiplier()
    {
        // NOTE: GlodBonus 철자 확인 필요. 실제 enum명이 GlodBonus면 그대로 두세요.
        return 1f + PermanentStatManager.GetValue(PermanentStatType.GlodBonus);
    }

    // --- Runtime upgrades API (used by level-up options) ---
    public void ApplyRuntimeUpgrade(StatType type, float value, bool isPercent)
    {
        switch (type)
        {
            case StatType.MoveSpeed:
                if (isPercent) rtMoveSpeedMul *= (1f + value);
                else rtMoveSpeedAdd += value;
                break;

            case StatType.GlobalDamage:
                if (isPercent) rtGlobalDamageMul *= (1f + value);
                else rtGlobalDamageMul += value; // usually percent path only
                break;

            case StatType.CooldownReduction:
                {
                    // +r% CDR => cooldown *= (1 - r)
                    float factor = isPercent ? (1f - value) : Mathf.Max(0f, 1f - value);
                    rtCooldownMul = Mathf.Clamp(rtCooldownMul * factor, 0.1f, 10f);
                }
                break;

            case StatType.GlobalFireRate:
                if (isPercent) rtGlobalFireRateMul *= (1f + value);
                else rtGlobalFireRateMul += value;
                break;

            case StatType.ProjectileSpeed:
                if (isPercent) rtProjectileSpeedMul *= (1f + value);
                else rtProjectileSpeedMul += value;
                break;

            case StatType.PierceAdd:
                rtPierceAdd += isPercent ? value : value; // pierce는 보통 정수 가산
                break;

            case StatType.MaxHP:
                if (isPercent) rtMaxHpAdd += Mathf.RoundToInt(baseMaxHealth * value);
                else rtMaxHpAdd += value;
                break;

            case StatType.Regen:
                if (isPercent) rtRegenAdd += baseRegen * value;
                else rtRegenAdd += value;
                break;

            case StatType.PickupRange:
                if (isPercent) rtPickupRangeAdd += rtPickupRangeAdd * value; // 필요 시 base값 참조로 교체
                else rtPickupRangeAdd += value;
                break;
        }

        RecalculateAll();
    }

    // --- Recalc totals once after any change ---
    public void RecalculateAll()
    {
        PullPermanent();

        // Totals = base × perm × runtime + runtime_add
        TotalDamage = baseDamage * permDamageMul * rtGlobalDamageMul;
        TotalMoveSpeed = (baseMoveSpeed + rtMoveSpeedAdd) * permMoveSpeedMul * rtMoveSpeedMul;
        TotalMaxHealth = (baseMaxHealth * permMaxHpMul) + rtMaxHpAdd;
        TotalRegen = (baseRegen + permRegenAdd) + rtRegenAdd;

        OnStatsChanged?.Invoke();
        //PlayerHealth.Instance.HandleStatsChanged();
    }

    // --- Global multipliers for weapons/projectiles ---
    public float GetGlobalDamageMul() => permDamageMul * rtGlobalDamageMul;
    public float GetGlobalFireRateMul() => rtGlobalFireRateMul;
    public float GetCooldownMul() => rtCooldownMul;
    public float GetProjectileSpeedMul() => rtProjectileSpeedMul;
    public float GetPierceAdd() => rtPierceAdd;

    // Movement helpers if weapons need it for effects
    public float GetMoveSpeed() => TotalMoveSpeed;

}
