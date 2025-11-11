using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugUI : MonoBehaviour
{
    //public void ResetAll()
    //{
    //    //PlayerPrefs.DeleteAll();
    //    PlayerPrefs.Save();
    //    Debug.Log("PlayerPrefs (stat + gold) Reset");
    //    //GoldManager.Instance.SetGold(0);
    [SerializeField] private Button resetButton;
        if (resetButton != null)
        {
            resetButton.interactable = false;
        }
    //    // 즉시 UI 반영
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
            Debug.LogWarning("ResetUpgradesAndRefund: allStatUpgrades 비어 있음");
            return;
        }

        // 1) 현재 누적 투자금 계산
        int refund = 0;
        foreach (var data in allStatUpgrades)
        {
            if (data == null) continue;
            int level = PermanentStatManager.GetLevel(data.statType);
            for (int i = 0; i < level; i++)
                refund += data.GetCost(i);      // 0→1, 1→2, ... 단계별 비용 합산
        }

        // 2) 업그레이드 레벨 전부 0으로 초기화
        foreach (var data in allStatUpgrades)
        {
            if (data == null) continue;
            PermanentStatManager.SetLevel(data.statType, 0);
        }

        // 3) 골드 환급 지급 (보유 골드는 유지 + 환급만 추가)
        GoldManager.Instance.AddGold(refund);

        // 4) 저장 및 UI 갱신
        PlayerPrefs.Save();
        RefreshAllStatItems();
        //UIManager.Instance.UpdateGoldUI(GoldManager.Instance.Gold);

        Debug.Log($"[Reset] 업그레이드 초기화 완료. 환급: {refund} G");
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
