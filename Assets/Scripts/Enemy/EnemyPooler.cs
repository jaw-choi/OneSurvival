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
            // ===== 추가 시작 =====
            // Ensure pooled instance always has EnemyData assigned
            var eb = obj.GetComponent<EnemyBase>();
            if (eb != null && eb.data == null)
                eb.data = data;

            obj.name = $"{data.name}_Pooled_{i}";
            // ===== 추가 끝 =====
            pool.Add(obj);
        }
    }

    public GameObject GetEnemy()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                // ===== 추가 시작 =====
                // Defensive: re-assign data if something cleared it
                var eb = pool[i].GetComponent<EnemyBase>();
                if (eb != null && eb.data == null)
                    eb  .data = data;
                // ===== 추가 끝 =====
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

    // ===== 추가 시작 =====
    // Quality-of-life: spawn with position/rotation before activation (prevents OnEnable using wrong pos)
    //public GameObject GetEnemyAt(Vector3 pos, Quaternion rot)
    //{
    //    var go = GetEnemy();
    //    if (go != null)
    //    {
    //        go.transform.SetPositionAndRotation(pos, rot);
    //        Physics2D.SyncTransforms();
    //    }
    //    return go;
    //}
    public GameObject GetEnemyAt(Vector3 pos, Quaternion rot)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            var go = pool[i];
            if (!go.activeInHierarchy)
            {
                var eb = go.GetComponent<EnemyBase>();
                if (eb && eb.data == null) eb.data = data;

                // 1) 먼저 좌표/회전
                go.transform.SetPositionAndRotation(pos, rot);
                Physics2D.SyncTransforms();

                // 2) 그 다음 활성화 → OnEnable에서 HP/상태 초기화
                go.SetActive(true);
                return go;
            }
        }
        return null;
    }
    // ===== 추가 끝 =====
}
