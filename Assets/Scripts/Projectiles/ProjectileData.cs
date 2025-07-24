using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData", order = 1)]
public class ProjectileData : ScriptableObject
{
    [Header("General")]
    public string projectileName;
    public Sprite sprite;
    public float speed;
    public float damage;
    public float lifetime;
    public int pierceCount;

    [Header("Behavior")]
    public ProjectileMoveType moveType;
    public ProjectileHitType hitType;
    public float aoeRadius;  // AoE Àü¿ë

    [Header("Special")]
    public GameObject hitEffectPrefab;
    public AudioClip hitSFX;
}
