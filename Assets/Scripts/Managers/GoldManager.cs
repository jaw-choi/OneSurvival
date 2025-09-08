using UnityEngine;
using UnityEngine.Events;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    public int Gold { get; private set; }

    public UnityEvent<int> OnGoldChanged = new UnityEvent<int>();

    private const string GOLD_KEY = "PlayerGold";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGold();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        SaveGold();
        OnGoldChanged.Invoke(Gold);
    }

    public bool SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            SaveGold();
            OnGoldChanged.Invoke(Gold);
            return true;
        }
        return false;
    }

    private void SaveGold()
    {
        if (UserInfo.IsLoggedIn && BackendGameData.Instance != null)
        {
            // ���� ������Ʈ
            BackendGameData.Instance.UserGameData.gold = Gold;
            BackendGameData.Instance.GameDataUpdate();
        }
        else
        {
            // ���� ����
            PlayerPrefs.SetInt(GOLD_KEY, Gold);
            PlayerPrefs.Save();
        }
    }
    public void SetGold(int amount)
    {
        // ���� ����
        Gold = Mathf.Max(0, amount);
        SaveGold();
        OnGoldChanged.Invoke(Gold);
    }

    // ��� ���� �ʱ�ȭ(���� ��ư���� ȣ��)
    public void ResetGold()
    {
        SetGold(0);
    }
    public void LoadGold()
    {
        //Gold = GameResultData.Instance.totalGold;
        if (UserInfo.IsLoggedIn && BackendGameData.Instance != null)
        {
            // ���� ������ ���
            Gold = BackendGameData.Instance.UserGameData.gold;
        }
        else
        {
            // ���� PlayerPrefs ���
            Gold = PlayerPrefs.GetInt(GOLD_KEY, 0);
        }
    }
}
