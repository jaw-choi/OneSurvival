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
            // 서버 업데이트
            BackendGameData.Instance.UserGameData.gold = Gold;
            BackendGameData.Instance.GameDataUpdate();
        }
        else
        {
            // 로컬 저장
            PlayerPrefs.SetInt(GOLD_KEY, Gold);
            PlayerPrefs.Save();
        }
    }
    public void SetGold(int amount)
    {
        // 음수 방지
        Gold = Mathf.Max(0, amount);
        SaveGold();
        OnGoldChanged.Invoke(Gold);
    }

    // 골드 완전 초기화(리셋 버튼에서 호출)
    public void ResetGold()
    {
        SetGold(0);
    }
    public void LoadGold()
    {
        //Gold = GameResultData.Instance.totalGold;
        if (UserInfo.IsLoggedIn && BackendGameData.Instance != null)
        {
            // 서버 데이터 사용
            Gold = BackendGameData.Instance.UserGameData.gold;
        }
        else
        {
            // 로컬 PlayerPrefs 사용
            Gold = PlayerPrefs.GetInt(GOLD_KEY, 0);
        }
    }
}
