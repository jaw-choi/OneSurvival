using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    void Start()
    {
        if (GoldManager.Instance != null)
        {
            // ��� ���� �̺�Ʈ ����
            GoldManager.Instance.OnGoldChanged.AddListener(UpdateGoldText);

            // �ʱ� ǥ�� (GoldManager ���ο��� ����/������ �̹� �������ֹǷ� �״�� ���)
            UpdateGoldText(GoldManager.Instance.Gold);
        }
    }

    void UpdateGoldText(int gold)
    {
        if (goldText)
        {
            if (UserInfo.IsLoggedIn && BackendGameData.Instance != null)
            {
                // ���� ������ ���
                goldText.text = $"Gold : {BackendGameData.Instance.UserGameData.gold}";

            }
            else
            {
                // ���� PlayerPrefs ���
                goldText.text = $"Gold : {GoldManager.Instance.Gold}";

            }
        }
    }

    void OnDestroy()
    {
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged.RemoveListener(UpdateGoldText);
    }
}
