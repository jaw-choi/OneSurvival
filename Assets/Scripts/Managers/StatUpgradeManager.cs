using UnityEngine;

public class StatUpgradeManager : MonoBehaviour
{
    public StatUpgradeData[] upgradeList;
    public GameObject upgradeItemPrefab;
    public Transform contentParent;

    void Start()
    {
        foreach (var data in upgradeList)
        {
            var go = Instantiate(upgradeItemPrefab, contentParent);
            var item = go.GetComponent<StatUpgradeUIItem>();
            item.Init(data);
        }
    }
}
