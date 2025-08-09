// Assets/Scripts/Pooling/EnemyPoolHub.cs
using UnityEngine;
using System.Collections.Generic;

public class EnemyPoolHub : MonoBehaviour
{
    public List<EnemyPooler> poolers = new();

    private readonly Dictionary<EnemyData, EnemyPooler> map = new();

    void Awake()
    {
        map.Clear();
        foreach (var p in poolers)
        {
            if (p == null || p.data == null) continue;
            if (!map.ContainsKey(p.data))
                map.Add(p.data, p);
        }
    }

    public GameObject Spawn(EnemyData data, Vector3 pos, Quaternion rot)
    {
        if (data == null) return null;
        if (!map.TryGetValue(data, out var pooler) || pooler == null)
        {
            Debug.LogWarning($"EnemyPoolHub: No pooler found for {data.name}");
            return null;
        }

        var go = pooler.GetEnemy();
        if (go != null)
        {
            go.transform.SetPositionAndRotation(pos, rot);
        }
        return go;
    }
}
