using UnityEngine;
using System.Collections.Generic;

public class EnemyPooler : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int poolSize = 30;
    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetEnemy()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        // Ǯ�� ���� �� ������ ���� ����ų�, null ��ȯ(Ȯ�� ����)
        return null;
    }
}
