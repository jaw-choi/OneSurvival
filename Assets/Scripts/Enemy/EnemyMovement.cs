using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.PlayerTransform == null) return;

        Vector2 playerPos = GameManager.Instance.PlayerTransform.position;
        Vector2 enemyPos = this.transform.position;
        Vector2 direction = (playerPos - enemyPos).normalized;
        //player - this  == this to player
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
