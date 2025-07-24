using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 2;
    private int pierceCount;
    Vector2 dir;

    public void SetData(float damage, int pierce)
    {
        this.damage = damage;
        this.pierceCount = pierce;
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
            col.GetComponent<EnemyHealth>().TakeDamage(damage);
            pierceCount--;
            if (pierceCount <= 0)
                Destroy(gameObject);
        }
    }
}
