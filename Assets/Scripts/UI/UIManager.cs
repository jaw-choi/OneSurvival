using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TextMeshProUGUI goldText;

    private const string GOLD_KEY = "PlayerGold";

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // PlayerPrefs에서 바로 골드 불러오기
        int gold = PlayerPrefs.GetInt(GOLD_KEY, 0);
        UpdateGoldUI(gold);

        // 이후에 GoldManager에서 이벤트 연결도 가능
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged.AddListener(UpdateGoldUI);
    }

    public void UpdateGoldUI(int gold)
    {
        if (goldText != null)
            goldText.text = "Gold : " + gold;
        else
            Debug.LogWarning("goldText가 할당되지 않았습니다.");
    }

    void OnDestroy()
    {
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged.RemoveListener(UpdateGoldUI);
    }
}
