using System.Collections.Generic;
using UnityEngine;

public class UpgradeRoller : MonoBehaviour
{
    public static UpgradeRoller Instance;

    [Header("Pools (mix weapons and stats)")]
    public List<UpgradeOptionSO> options = new List<UpgradeOptionSO>();

    [Header("Roll Settings")]
    public int rollCount = 3;

    private System.Random rng;

    void Awake()
    {
        Instance = this;
        rng = new System.Random();
    }

    public List<UpgradeOptionSO> Roll()
    {
        // Filter options that can still roll
        var pool = new List<UpgradeOptionSO>();
        foreach (var o in options)
            if (o != null && o.CanRoll())
                pool.Add(o);

        // Sample without replacement
        var result = new List<UpgradeOptionSO>();
        while (result.Count < rollCount && pool.Count > 0)
        {
            int idx = rng.Next(pool.Count);
            result.Add(pool[idx]);
            pool.RemoveAt(idx);
        }
        return result;
    }

    public void Apply(UpgradeOptionSO opt)
    {
        opt?.Apply();
    }
}
