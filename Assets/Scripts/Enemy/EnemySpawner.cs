using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPooler enemyPooler;
    public float spawnInterval = 1.5f;
    private float timer = 0f;
    public float spawnDistance = 4f; // 화면 밖(월드 기준) 거리

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnEnemyOutsideScreen();
        }
    }

    void SpawnEnemyOutsideScreen()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsGameOver)
            return;

        // 화면 중심(플레이어 위치) 기준, 랜덤 방향으로 spawnDistance만큼 떨어진 곳에 스폰

        Vector2 playerPos = GameManager.Instance.PlayerTransform.position;
        float angle = Random.Range(0f, 1f * Mathf.PI);
        Vector2 spawnDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 spawnPos = playerPos + spawnDir * spawnDistance;

        GameObject enemy = enemyPooler.GetEnemy();
        if (enemy != null)
        {
            enemy.transform.position = spawnPos;
            enemy.transform.rotation = Quaternion.identity;
            // 적 리셋(체력 등) 필요시 초기화 함수 호출
        }
    }
}
