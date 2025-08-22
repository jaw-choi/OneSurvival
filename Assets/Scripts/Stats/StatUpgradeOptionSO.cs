using UnityEngine;

[CreateAssetMenu(fileName = "StatOption", menuName = "Roguelike/Upgrade Option/Stat")]
public class StatUpgradeOptionSO : UpgradeOptionSO
{
    public StatType statType;
    public bool isPercent = true;        // true: 0.10 means +10%
    public float[] perLevelValues;       // value per step (length = maxLevel is recommended)

    private void OnValidate()
    {
        kind = UpgradeKind.Stat;
        if (string.IsNullOrEmpty(id))
            id = $"stat_{statType}";
        if (string.IsNullOrEmpty(displayName))
            displayName = statType.ToString();
    }

    public override bool CanRoll()
    {
        if (StatUpgradeManager.Instance == null) return false;
        int cur = GetCurrentLevel();
        int limit = Mathf.Min(maxLevel, perLevelValues != null ? perLevelValues.Length : maxLevel);
        return cur < limit;
    }

    public override int GetCurrentLevel()
    {
        return StatUpgradeManager.Instance != null
            ? StatUpgradeManager.Instance.GetLevel(id)
            : 0;
    }

    public override void Apply()
    {
        if (StatUpgradeManager.Instance == null || PlayerStats.Instance == null) return;

        int cur = GetCurrentLevel();
        int idx = Mathf.Clamp(cur, 0, (perLevelValues?.Length ?? 1) - 1);
        float step = (perLevelValues != null && perLevelValues.Length > 0) ? perLevelValues[idx] : 0f;

        // Delegate real stat change into StatUpgradeManager (keeps rules centralized)
        StatUpgradeManager.Instance.ApplyStat(statType, step, isPercent);
        StatUpgradeManager.Instance.IncreaseLevel(id, 1);
    }
}
