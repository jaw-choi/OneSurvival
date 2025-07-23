using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    private float cooldownTimer;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            Fire();
            cooldownTimer = weaponData.attackCooldown;
        }
    }

    void Fire()
    {
        if (weaponData.projectilePrefab != null)
        {
            GameObject projectile = Instantiate(
                weaponData.projectilePrefab,
                transform.position,
                transform.rotation
            );

            Projectile projScript = projectile.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.SetData(weaponData.damage, weaponData.pierceCount);
            }
        }
    }
}
