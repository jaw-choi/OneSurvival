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

    public List<WeaponResult> weaponResults = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
