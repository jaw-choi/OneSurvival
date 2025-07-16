using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 5;
    Vector2 dir;

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
            Destroy(gameObject);
        }
    }
}
