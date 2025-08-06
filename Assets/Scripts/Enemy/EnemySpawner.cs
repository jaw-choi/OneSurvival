using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPooler enemyPooler;
    public float spawnInterval = 1.5f;
    public float initialSpawnInterval = 1.5f;
    private float timer = 0f;
    public float spawnDistance = 4f; // ȭ�� ��(���� ����) �Ÿ�
    public float minSpawnInterval = 0.3f;
    public float difficultyRampTime = 300f; // 5�� ���� ���̵� ����

    void Update()
    {
        float t = Mathf.Clamp01(GameManager.Instance.ElapsedTime / difficultyRampTime);
        spawnInterval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, t);

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

        // ȭ�� �߽�(�÷��̾� ��ġ) ����, ���� �������� spawnDistance��ŭ ������ ���� ����

        Vector2 playerPos = GameManager.Instance.PlayerTransform.position;
        float angle = Random.Range(0f, 1f * Mathf.PI);
        Vector2 spawnDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 spawnPos = playerPos + spawnDir * spawnDistance;

        GameObject enemy = enemyPooler.GetEnemy();
        if (enemy != null)
        {
            enemy.transform.position = spawnPos;
            enemy.transform.rotation = Quaternion.identity;
            // �� ����(ü�� ��) �ʿ�� �ʱ�ȭ �Լ� ȣ��
        }
    }
}
