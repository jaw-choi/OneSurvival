// File: Assets/Scripts/Data/EnemyData.cs
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData", fileName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Prefab & Visual")]
    [Tooltip("The prefab used for pooling/spawning")]
    public GameObject prefab;                       // enemy prefab (has EnemyBase, Animator, SpriteRenderer, etc.)
    [Tooltip("Override sprite on enable (optional)")]
    public Sprite enemySprite;                      // optional override for SpriteRenderer
    [Tooltip("Override animator controller on enable (optional)")]
    public RuntimeAnimatorController animatorController; // optional override for Animator

    [Header("Pool Settings")]
    [Min(0)] public int initialPoolSize = 20;       // initial size per pooler
    public bool expandable = true;                  // allow expansion when pool is exhausted
    [Min(0)] public int maxExpand = 100;            // expansion limit

    [Header("Stats")]
    [Tooltip("Max health point")]
    public float maxHP = 10f;
    [Tooltip("Movement speed (units/sec)")]
    public float moveSpeed = 2f;
    [Tooltip("Contact damage to player")]
    public float contactDamage = 1f;

    [Header("Drops")]
    [Tooltip("Experience amount dropped on death")]
    public int expDrop = 1;
    [Tooltip("Optional exp gem prefab to spawn on death")]
    public GameObject expGemPrefab;                 // used by EnemyBase.Die()

    [Header("Spawn/Progression")]
    [Tooltip("Relative weight in weighted random selection")]
    public int spawnWeight = 1;                     // used by weighted pickers

    [Header("Misc (Optional Behaviors)")]
    [Tooltip("Optional: knockback resistance (0 = none, 1 = full)")]
    [Range(0f, 1f)] public float knockbackResist = 0f;
    [Tooltip("Optional: idle/aggro detection radius")]
    public float detectionRadius = 6f;

    // Called when values change in the inspector
    private void OnValidate()
    {
        // Clamp/guard common values
        if (initialPoolSize < 0) initialPoolSize = 0;
        if (maxExpand < 0) maxExpand = 0;
        if (spawnWeight < 0) spawnWeight = 0;
        if (maxHP < 1f) maxHP = 1f;
        if (moveSpeed < 0f) moveSpeed = 0f;
        if (contactDamage < 0f) contactDamage = 0f;
        if (expDrop < 0) expDrop = 0;

        // Auto-name convenience
        if (prefab != null && string.IsNullOrEmpty(name))
        {
            // no-op (Unity manages asset name), but could set a display field if you add one
        }
    }
}
