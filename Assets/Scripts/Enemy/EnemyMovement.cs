using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyBase enemyBase;
    private SpriteRenderer spriteRenderer;
    public float speed = 1f;

    void Start()
    {
        // Get the SpriteRenderer component
        enemyBase = GetComponent<EnemyBase>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyBase.isDead) return; // if enemy dead, do not move
        if (GameManager.Instance.PlayerTransform == null) return;

        Vector2 direction = (GameManager.Instance.PlayerTransform.position - transform.position).normalized;

        // Get horizontal input (-1 for left, 1 for right)

        if (direction.x > 0.01f) spriteRenderer.flipX = false;
        else if (direction.x < 0.01f) spriteRenderer.flipX = true;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        //player - this  == this to player
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
