using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    void Start()
    {
        if (GoldManager.Instance != null)
        {
            // 골드 변경 이벤트 구독
            GoldManager.Instance.OnGoldChanged.AddListener(UpdateGoldText);

            // 초기 표시 (GoldManager 내부에서 서버/로컬을 이미 구분해주므로 그대로 사용)
            UpdateGoldText(GoldManager.Instance.Gold);
        }
    }

    void UpdateGoldText(int gold)
    {
        if (goldText)
        {
            if (UserInfo.IsLoggedIn && BackendGameData.Instance != null)
            {
                // 서버 데이터 사용
                goldText.text = $"Gold : {BackendGameData.Instance.UserGameData.gold}";

            }
            else
            {
                // 로컬 PlayerPrefs 사용
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
