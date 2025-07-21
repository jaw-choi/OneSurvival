using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float speed = 2f;

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.PlayerTransform == null) return;

        Vector2 playerPos = GameManager.Instance.PlayerTransform.position;
        Vector2 enemyPos = this.transform.position;
        Vector2 direction = (playerPos - enemyPos).normalized;

        // Get horizontal input (-1 for left, 1 for right)

        if (direction.x > 0.01f) spriteRenderer.flipX = false;
        else if (direction.x < 0.01f) spriteRenderer.flipX = true;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        //player - this  == this to player
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
