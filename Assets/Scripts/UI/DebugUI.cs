using UnityEngine;
using TMPro;

public class DebugUI : MonoBehaviour
{
    //public void ResetAll()
    //{
    //    //PlayerPrefs.DeleteAll();
    //    PlayerPrefs.Save();
    //    Debug.Log("PlayerPrefs (stat + gold) Reset");
    //    //GoldManager.Instance.SetGold(0);
    //    // ��� UI �ݿ�
    //    //UIManager.Instance.UpdateGoldUI(0);
    //    RefreshAllStatItems();
    //}
    [SerializeField] private StatUpgradeData[] allStatUpgrades;

    private void Awake()
    {
        allStatUpgrades = StatUpgradeManager.Instance.upgradeList;
    }
    public void ResetUpgradesAndRefund()
    {
        if (allStatUpgrades == null || allStatUpgrades.Length == 0)
        {
            Debug.LogWarning("ResetUpgradesAndRefund: allStatUpgrades ��� ����");
            return;
        }

        // 1) ���� ���� ���ڱ� ���
        int refund = 0;
        foreach (var data in allStatUpgrades)
        {
            if (data == null) continue;
            int level = PermanentStatManager.GetLevel(data.statType);
            for (int i = 0; i < level; i++)
                refund += data.GetCost(i);      // 0��1, 1��2, ... �ܰ躰 ��� �ջ�
        }

        // 2) ���׷��̵� ���� ���� 0���� �ʱ�ȭ
        foreach (var data in allStatUpgrades)
        {
            if (data == null) continue;
            PermanentStatManager.SetLevel(data.statType, 0);
        }

        // 3) ��� ȯ�� ���� (���� ���� ���� + ȯ�޸� �߰�)
        GoldManager.Instance.AddGold(refund);

        // 4) ���� �� UI ����
        PlayerPrefs.Save();
        RefreshAllStatItems();
        UIManager.Instance.UpdateGoldUI(GoldManager.Instance.Gold);

        Debug.Log($"[Reset] ���׷��̵� �ʱ�ȭ �Ϸ�. ȯ��: {refund} G");
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
