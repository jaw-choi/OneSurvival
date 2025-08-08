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
    public GameObject weaponResultItemPrefab; // ���� �ϳ��� �� ��

    void Start()
    {
        var result = GameResultData.Instance;

        playTimeText.text = "�÷��� �ð� : " + FormatTime(result.playTime);
        goldText.text = "���� ��� : " + result.totalGold.ToString();
        levelText.text = $"Lv. {result.playerLevel}";
        killCountText.text = $"{result.enemyKillCount} Kills";
        characterNameText.text = $"ĳ���� : {result.characterName}";
        mapNameText.text = $"�� : {result.mapName}";

        foreach (var w in result.weaponResults)
        {
            GameObject item = Instantiate(weaponResultItemPrefab, weaponListParent);
            var tmp = item.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = $"���� {w.weaponName} - Lv.{w.weaponLevel} \n ������ : {w.totalDamage} \n �ʴ� ������ {w.dps:F1} \n �����ð� : {w.heldTime:F1}��";
        }
    }

    string FormatTime(float time)
    {
        int min = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);
        return $"{min:D2}:{sec:D2}";
    }
}
