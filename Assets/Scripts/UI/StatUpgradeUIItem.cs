using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatUpgradeUIItem : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text levelText;
    //public TMP_Text effectText;
    public TMP_Text costText;
    public Button upgradeButton;

    private StatUpgradeData upgradeData;

    public void Init(StatUpgradeData data)
    {
        upgradeData = data;
        Refresh();
        upgradeButton.onClick.AddListener(OnUpgrade);
    }

    public void Refresh()
    {
        if (upgradeData == null)
            return;
        int level = PermanentStatManager.GetLevel(upgradeData.statType);
        int max = upgradeData.maxLevel;

        nameText.text = upgradeData.displayName;
        levelText.text = $"Lv.{level} / {max}";
        //effectText.text = $"Effect: {upgradeData.GetValue(level)}";
        costText.text = $"{upgradeData.GetCost(level)} G";

        bool canUpgrade = level < max && GoldManager.Instance.Gold >= upgradeData.GetCost(level);
        upgradeButton.interactable = canUpgrade;
    }

    private void OnUpgrade()
    {
        int level = PermanentStatManager.GetLevel(upgradeData.statType);
        if (level >= upgradeData.maxLevel) return;

        int cost = upgradeData.GetCost(level);
        if (GoldManager.Instance.SpendGold(cost))
        {
            PermanentStatManager.SetLevel(upgradeData.statType, level + 1);
            Refresh();
            UIManager.Instance.UpdateGoldUI(GoldManager.Instance.Gold);
        }
    }

    void OnEnable()
    {
        if (GoldManager.Instance != null)
        {
            // 골드 변경 시 자동 새로고침
            GoldManager.Instance.OnGoldChanged.AddListener(OnGoldChanged);
        }
    }

    void OnDisable()
    {
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged.RemoveListener(OnGoldChanged);
        }
    }

    private void OnGoldChanged(int currentGold)
    {
        Refresh(); // 골드 기준으로 버튼 interactable 갱신
    }
}
