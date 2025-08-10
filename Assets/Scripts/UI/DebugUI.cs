using UnityEngine;
using TMPro;

public class DebugUI : MonoBehaviour
{
    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs (stat + gold) Reset");
        GoldManager.Instance.SetGold(0);
        // 즉시 UI 반영
        UIManager.Instance.UpdateGoldUI(0);
        RefreshAllStatItems();
    }

    public void AddGold10k()
    {
        GoldManager.Instance.AddGold(10000);
        Debug.Log("Gold +10,000");
    }
    private void RefreshAllStatItems()
    {
        foreach (var item in FindObjectsByType<StatUpgradeUIItem>(FindObjectsSortMode.None))
        {
            item.Refresh();
        }
    }
}
