using UnityEngine;

public enum UpgradeKind { Weapon, Stat }

public abstract class UpgradeOptionSO : ScriptableObject
{
    [Header("Common")]
    public string id;                   // unique key for runtime levels
    public UpgradeKind kind;
    public string displayName;
    public Sprite icon;
    [TextArea] public string shortDescription;
    [Min(1)] public int maxLevel = 5;

    // Can this option still appear?
    public abstract bool CanRoll();
    // Current level tracked by runtime systems
    public abstract int GetCurrentLevel();
    // Apply effect & increase its runtime level
    public abstract void Apply();
}
