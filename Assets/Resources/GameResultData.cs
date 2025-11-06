using UnityEngine;
using System.Collections.Generic;

public class GameResultData : MonoBehaviour
{
    public static GameResultData Instance { get; private set; }

    public float playTime;
    public int totalGold;
    public int playerLevel;
    public int enemyKillCount;
    public string characterName;
    public string mapName;

    public int totalEnemyKillCount; // 전체 누적 킬 수
    private const string TOTAL_KILL_KEY = "TotalEnemyKillCount";

    public List<WeaponResult> weaponResults = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTotalKillCount();
        }
        else Destroy(gameObject);
    }
    public void Reset()
    {
        playTime = 0;
        totalGold = 0;
        playerLevel = 0;
        enemyKillCount = 0;
        characterName = "";
        mapName = "";
        weaponResults.Clear();
    }
    public void SaveTotalKillCount()
    {
        PlayerPrefs.SetInt(TOTAL_KILL_KEY, totalEnemyKillCount);
        PlayerPrefs.Save();
    }

    public void LoadTotalKillCount()
    {
        if (UserInfo.IsLoggedIn && BackendGameData.Instance != null)
        {
            totalEnemyKillCount = BackendGameData.Instance.UserGameData.score;
        }
        else
        {
            totalEnemyKillCount = PlayerPrefs.GetInt(TOTAL_KILL_KEY, 0);
        }
    }
    public void AddToTotalKillCount(int count)
    {
        totalEnemyKillCount += count;
        SaveTotalKillCount();
    }
}



[System.Serializable]
public class WeaponResult
{
    public string weaponName;
    public int weaponLevel;
    public float totalDamage;
    public float heldTime;
    public float dps => heldTime > 0 ? totalDamage / heldTime : 0;
}
