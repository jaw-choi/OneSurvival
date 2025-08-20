using UnityEngine;

public class AoeFireBehaviour : IWeaponFireBehaviour
{
    private Transform ownerTransform;
    private Weapon weapon;

    public AoeFireBehaviour(Weapon weapon)
    {
        this.weapon = weapon;
        this.ownerTransform = weapon.transform;
    }

    public void Fire(Vector2 position, Vector2 direction, WeaponData data)
    {
        Vector2 center = ownerTransform.position;
        float radius = data.projectileData.aoeRadius;
        float damage = data.projectileData.damage * weapon.GetDamageMultiplier();

        Collider2D[] enemies = Physics2D.OverlapCircleAll(center, radius);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
                enemy.GetComponent<EnemyBase>()?.TakeDamage(damage);
        }

        if (data.projectileData.hitEffectPrefab != null)
            Object.Instantiate(data.projectileData.hitEffectPrefab, center, Quaternion.identity);


    }
}
