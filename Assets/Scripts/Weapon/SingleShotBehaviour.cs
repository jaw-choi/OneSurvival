using UnityEngine;

public class SingleShotBehaviour : IWeaponFireBehaviour
{
    public void Fire(Vector2 position, Vector2 direction, WeaponData data)
    {
        GameObject proj = Object.Instantiate(data.projectilePrefab, position, Quaternion.identity);
        proj.GetComponent<Projectile>().Init(direction.normalized * data.projectileData.speed);
    }
}
