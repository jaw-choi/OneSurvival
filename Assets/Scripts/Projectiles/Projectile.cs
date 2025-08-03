using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    private int pierceCount;
    Vector2 dir;

    public void SetData(ProjectileData baseData, Weapon weapon)
    {
        damage = baseData.damage * weapon.GetDamageMultiplier();
        speed = baseData.speed * weapon.GetSpeedMultiplier();
        pierceCount = baseData.pierceCount + Mathf.RoundToInt(weapon.GetPierceBonus());

        Destroy(gameObject, baseData.lifetime);
    }

    public void Init(Vector2 direction)
    {
        dir = direction.normalized;
        Destroy(gameObject, 1f); // 1초 뒤 자동 파괴
    }

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Debug.Log("hit the Enemy");
            col.GetComponent<EnemyBase>().TakeDamage(damage);
            pierceCount--;
            if (pierceCount <= 0)
                Destroy(gameObject);
        }
    }
}
