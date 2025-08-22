using System.Collections.Generic;
using UnityEngine;

public class StatUpgradeManager : MonoBehaviour
{

    public StatUpgradeData[] upgradeList;
    public GameObject upgradeItemPrefab;
    public Transform contentParent;

    public static StatUpgradeManager Instance;

    private readonly Dictionary<string, int> levels = new Dictionary<string, int>();

    void Awake() { Instance = this; }

    void Start()
    {
        foreach (var data in upgradeList)
        {
            var go = Instantiate(upgradeItemPrefab, contentParent);
            var item = go.GetComponent<StatUpgradeUIItem>();
            item.Init(data);
        }
    }

    public int GetLevel(string id) => levels.TryGetValue(id, out var lv) ? lv : 0;
    public void IncreaseLevel(string id, int amount)
    {
        int cur = GetLevel(id);
        levels[id] = cur + amount;
    }

    public void ApplyStat(StatType type, float value, bool isPercent)
    {
        // Delegate real stat change to PlayerStats so that UI/other systems read single source of truth
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.ApplyRuntimeUpgrade(type, value, isPercent);
    }

}
