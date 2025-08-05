using UnityEngine;
using TMPro;

public class DebugUI : MonoBehaviour
{
    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs (stat + gold) Reset");

        // ��� UI �ݿ�
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
        foreach (var item in FindObjectsOfType<StatUpgradeUIItem>())
        {
            item.Refresh();
        }
    }
}
