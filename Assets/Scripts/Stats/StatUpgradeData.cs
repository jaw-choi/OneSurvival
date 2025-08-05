using UnityEngine;

[CreateAssetMenu(fileName = "StatUpgrade", menuName = "Scriptable Objects/StatUpgrade")]
public class StatUpgradeData : ScriptableObject
{
    public PermanentStatType statType;
    public string displayName;
    public int maxLevel = 10;
    public int[] costPerLevel;
    public float[] valuePerLevel;

    public int GetCost(int level)
    {
        return costPerLevel[Mathf.Clamp(level, 0, costPerLevel.Length - 1)];
    }

    public float GetValue(int level)
    {
        return valuePerLevel[Mathf.Clamp(level, 0, valuePerLevel.Length - 1)];
    }
}
