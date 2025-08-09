// Assets/Scripts/Pooling/EnemyPooler.cs
using UnityEngine;
using System.Collections.Generic;

public class EnemyPooler : MonoBehaviour
{
    // Use SO-only
    public EnemyData data;              // prefab, pool size, stats are in SO
    public int poolSize = 30;

    private readonly List<GameObject> pool = new();

    void Awake()
    {
        if (data == null || data.prefab == null)
        {
            Debug.LogError("EnemyPooler: EnemyData or prefab is missing.");
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(data.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetEnemy()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        // Optional: expand
        // var obj = Instantiate(data.prefab, transform);
        // obj.SetActive(true);
        // pool.Add(obj);
        // return obj;
        return null;
    }
}
