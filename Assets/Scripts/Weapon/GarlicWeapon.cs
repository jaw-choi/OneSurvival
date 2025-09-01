using UnityEngine;

public class GarlicWeapon : MonoBehaviour
{
    private Weapon weapon;
    private Transform player;
    private float tickTimer;
    private float tickInterval = 0.5f; // 반초마다 데미지

    private SpriteRenderer visual;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        weapon = GetComponent<Weapon>();
        visual = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (weapon == null || weapon.weaponData == null)
            return;

        tickTimer += Time.deltaTime;
        if (tickTimer >= tickInterval)
        {
            ApplyAreaDamage();
            tickTimer = 0f;
        }

        UpdateVisualRadius();
    }

    void ApplyAreaDamage()
    {
        float radius = weapon.weaponData.projectileData.aoeRadius;
        float damage = weapon.weaponData.projectileData.damage * weapon.GetDamageMultiplier();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var target in hits)
        {
            if (target.CompareTag("Enemy"))
                target.GetComponent<EnemyBase>()?.TakeDamage(damage, player);
        }
        weapon.AddDamage(damage);

    }

    void UpdateVisualRadius()
    {
        float radius = weapon.weaponData.projectileData.aoeRadius * weapon.GetSpeedMultiplier();
        if (visual != null)
        {
            float size = radius * 2f;
            visual.transform.localScale = new Vector3(size, size, 1f);
        }
    }
}
