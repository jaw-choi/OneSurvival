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

    [Header("Calculated Stats")]
    public float TotalDamage { get; private set; }
    public float TotalMoveSpeed { get; private set; }
    public float TotalMaxHealth { get; private set; }
    public float TotalRegen { get; private set; }

    void Awake()
    {
        Instance = this;
        ApplyPermanentUpgrades();
    }

    public void ApplyPermanentUpgrades()
    {
        TotalDamage = baseDamage * (1f + PermanentStatManager.GetValue(PermanentStatType.Damage));
        TotalMoveSpeed = baseMoveSpeed * (1f + PermanentStatManager.GetValue(PermanentStatType.MoveSpeed));
        TotalMaxHealth = baseMaxHealth * (1f + PermanentStatManager.GetValue(PermanentStatType.MaxHP));
        TotalRegen = baseRegen + PermanentStatManager.GetValue(PermanentStatType.Regen);
    }

    public float GetExpBonusMultiplier()
    {
        return 1f + PermanentStatManager.GetValue(PermanentStatType.ExpBonus);
    }
    public float GetGoldBonusMultiplier()
    {
        return 1f + PermanentStatManager.GetValue(PermanentStatType.GlodBonus);
    }
}
