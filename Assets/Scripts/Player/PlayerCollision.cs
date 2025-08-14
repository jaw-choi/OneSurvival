using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            Debug.Log("Collision!");
            if (playerHealth != null && !enemy.isDead)
            {
                playerHealth.TakeDamage(enemy.data.contactDamage);
            }
        }
    }
}
