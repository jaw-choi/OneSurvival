using UnityEngine;

public class SingleShotBehaviour : IWeaponFireBehaviour
{
    private Transform ownerTransform;
    private Weapon weapon;

    public SingleShotBehaviour(Weapon weapon)
    {
        this.weapon = weapon;
        this.ownerTransform = weapon.transform;
    }

    public void Fire(Vector2 position, Vector2 direction, WeaponData data)
    {
        GameObject proj = Object.Instantiate(data.projectilePrefab, ownerTransform.position, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.SetData(data.projectileData, weapon);
        projectile.Init(direction.normalized);
        Vector2 center = ownerTransform.position;
        if (data.projectileData.hitEffectPrefab != null)
            Object.Instantiate(data.projectileData.hitEffectPrefab, center, Quaternion.identity);

        if (data.projectileData.hitSFX != null)
            AudioSource.PlayClipAtPoint(data.projectileData.hitSFX, center);
    }
}
