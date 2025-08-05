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
        // PlayerPrefs���� �ٷ� ��� �ҷ�����
        int gold = PlayerPrefs.GetInt(GOLD_KEY, 0);
        UpdateGoldUI(gold);

        // ���Ŀ� GoldManager���� �̺�Ʈ ���ᵵ ����
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged.AddListener(UpdateGoldUI);
    }

    public void UpdateGoldUI(int gold)
    {
        if (goldText != null)
            goldText.text = "Gold : " + gold;
        else
            Debug.LogWarning("goldText�� �Ҵ���� �ʾҽ��ϴ�.");
    }

    void OnDestroy()
    {
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged.RemoveListener(UpdateGoldUI);
    }
}
