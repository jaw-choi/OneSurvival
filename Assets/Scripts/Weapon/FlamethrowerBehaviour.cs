using System.Collections;
using UnityEngine;

public class FlamethrowerBehaviour : IWeaponFireBehaviour
{
    private MonoBehaviour coroutineOwner;
    private Transform ownerTransform;
    private Weapon weapon;

    public FlamethrowerBehaviour(Weapon weapon)
    {
        this.weapon = weapon;
        this.ownerTransform = weapon.transform;
        this.coroutineOwner = weapon;
    }

    public void Fire(Vector2 position, Vector2 direction, WeaponData data)
    {
        coroutineOwner.StartCoroutine(FireFlame(direction));
        Vector2 center = ownerTransform.position;
        if (data.projectileData.hitEffectPrefab != null)
            Object.Instantiate(data.projectileData.hitEffectPrefab, center, Quaternion.identity);

        if (data.projectileData.hitSFX != null)
            AudioSource.PlayClipAtPoint(data.projectileData.hitSFX, center);
    }

    private IEnumerator FireFlame(Vector2 direction)
    {
        int flameShots = 5;
        float interval = 0.05f;

        for (int i = 0; i < flameShots; i++)
        {
            GameObject proj = Object.Instantiate(weapon.weaponData.projectilePrefab, ownerTransform.position, Quaternion.identity);
            Projectile projectile = proj.GetComponent<Projectile>();
            projectile.SetData(weapon.weaponData.projectileData, weapon);
            projectile.Init(direction.normalized);

            yield return new WaitForSeconds(interval);
        }
    }
}
