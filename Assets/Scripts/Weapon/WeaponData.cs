using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData", order = 2)]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject projectilePrefab;
    public ProjectileData projectileData;

    [Header("Fire Settings")]
    public WeaponFireType fireType;
    public float attackCooldown;
    public int burstCount;
    public float timeBetweenBurstShots;

    [Header("Range & Area")]
    public float attackRange;
    public float areaRadius;         // Flamethrower 전용
    public LayerMask enemyLayerMask; // 범위 감지용

    [Header("Visual & Sound")]
    public AudioClip attackSFX;
    public string attackAnimName;

    [Header("Stat")]
    public float damage;
    public float fireRate;
    public float projectileSpeed;
    public int pierceCount;

    //[Header("Level Up")]
    //public WeaponData nextLevelWeapon;
}
