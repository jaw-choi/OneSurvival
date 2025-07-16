using UnityEngine;

public class PlayerAutoAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float attackInterval = 1.5f;
    float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            timer = 0f;
            GameObject nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                Vector2 dir = (nearestEnemy.transform.position - transform.position).normalized;
                Fire(dir);
            }
        }
    }

    void Fire(Vector2 direction)
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Init(direction);
    }

    GameObject FindNearestEnemy()
    {
        // 간단 예시. 실전은 EnemyManager가 적 리스트 관리
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDist = float.MaxValue;
        GameObject nearest = null;
        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }
}
