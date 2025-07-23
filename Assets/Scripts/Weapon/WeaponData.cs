using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject projectilePrefab;
    /*
    public enum WeaponType { Melee, Ranged, Area }
    public WeaponType type;
    public AudioClip attackSFX;
    public string attackAnimName;
     */

    [Header("Stat")]
    public float damage;
    public float attackRange;
    public float attackCooldown;
    public int pierceCount;
    [Header("Level Up")]
    public WeaponData nextLevelWeapon;
}
