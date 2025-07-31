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
        PlayerPrefs.SetInt(GOLD_KEY, Gold);
        PlayerPrefs.Save();
    }

    private void LoadGold()
    {
        Gold = PlayerPrefs.GetInt(GOLD_KEY, 0); // ±âº»°ª 0
    }
}
