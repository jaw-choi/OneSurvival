using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultSceneManager : MonoBehaviour
{
    public TextMeshProUGUI playTimeText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI mapNameText;

    public Transform weaponListParent;
    public GameObject weaponResultItemPrefab; // 무기 하나당 한 줄

    void Start()
    {
        var result = GameResultData.Instance;

        playTimeText.text = "플레이 시간 : " + FormatTime(result.playTime);
        goldText.text = "얻은 골드 : " + result.totalGold.ToString();
        levelText.text = $"Lv. {result.playerLevel}";
        killCountText.text = $"{result.enemyKillCount} Kills";
        characterNameText.text = $"캐릭터 : {result.characterName}";
        mapNameText.text = $"맵 : {result.mapName}";

        foreach (var w in result.weaponResults)
        {
            GameObject item = Instantiate(weaponResultItemPrefab, weaponListParent);
            var tmp = item.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = $"무기 {w.weaponName} - Lv.{w.weaponLevel} \n 데미지 : {w.totalDamage} \n 초당 데미지 {w.dps:F1} \n 소유시간 : {w.heldTime:F1}초";
        }
    }

    string FormatTime(float time)
    {
        int min = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);
        return $"{min:D2}:{sec:D2}";
    }
}
