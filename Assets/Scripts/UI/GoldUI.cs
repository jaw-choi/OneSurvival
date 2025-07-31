using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    void Start()
    {
        GoldManager.Instance.OnGoldChanged.AddListener(UpdateGoldText); 
        UpdateGoldText(GoldManager.Instance.Gold); // �ʱ� ǥ��
    }

    void UpdateGoldText(int gold)
    {
        goldText.text = $"Gold: {gold}";
    }

    void OnDestroy()
    {
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged.RemoveListener(UpdateGoldText);
    }
}
