using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData", order = 2)]
public class WeaponData : ScriptableObject
{

    public string weaponName;
    public Sprite weaponIcon;
    public GameObject weaponPrefab;      // 무기 자체 프리팹
    public GameObject projectilePrefab;
    public ProjectileData projectileData;
    public int currentLevel;

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
    //public AudioClip attackSFX;
    public string attackAnimName;

    [Header("Orbit Settings")]
    public int orbitCount = 1;
    public float orbitRadius = 1.5f;
    public float orbitSpeedDeg = 120f;
    public bool orbitFaceOutward = true;
    public float orbitDuration = 0f;
    public bool orbitUseRigidbodyMove = false;
    public bool orbitRandomStart = true;
    public bool orbitSpawnEase = true;
    public float orbitSpawnEaseTime = 0.12f;
    public bool orbitPersistent = true;


    [Header("Orbit Settings (per level)")]
    public float orbitSpeedBaseDeg = 120f;
    public int[] orbitCountPerLevel = new int[] { 1, 2, 3, 4, 5 };
    public float[] orbitSpeedDegPerLevel = new float[] { 120f, 140f, 160f, 180f, 200f };

    [Header("Level Scaling")]
    public float[] fireRateMultiplierPerLevel;
    public float[] damageMultiplierPerLevel;
    public float[] speedMultiplierPerLevel;
    public float[] pierceBonusPerLevel;

}
