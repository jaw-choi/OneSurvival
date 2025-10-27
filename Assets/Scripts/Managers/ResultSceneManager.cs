using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResultSceneManager : MonoBehaviour
{
    [Header("Result Texts")]
    public TextMeshProUGUI playTimeText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI mapNameText;

    [Header("Weapon List")]
    public Transform weaponListParent;
    public GameObject weaponResultItemPrefab;

    [Header("Gold Conversion Weights")]
    [Min(0)] public int goldPerSecond = 1;
    [Min(0)] public int goldPerKill = 2;
    [Min(0)] public float goldPerDamage = 0.01f;
    [Min(0)] public int goldPerWeaponLevel = 5;
    [Min(0)] public int clearBonus = 0;

    [Header("Animation")]
    [Min(0.1f)] public float awardAnimDuration = 1.5f;
    public bool autoStartAward = true;

    int runAwardGold;
    int bonusAwardGold;
    int totalAwardGold;
    int savedGoldBefore;
    [SerializeField]
    private RankRegister rankRegister;

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

        savedGoldBefore = GoldManager.Instance.Gold;
        runAwardGold = Mathf.Max(0, result.totalGold);
        bonusAwardGold = CalculateBonus(result);
        totalAwardGold = runAwardGold + bonusAwardGold + Mathf.Max(0, clearBonus);
        //GoldManager.Instance.AddGold(totalAwardGold);
        if (BackendGameData.Instance != null)
        {
            rankRegister.Process(result.enemyKillCount);
            BackendGameData.Instance.UserGameData.score = result.enemyKillCount;
            BackendGameData.Instance.UserGameData.gold += totalAwardGold;
            BackendGameData.Instance.UserGameData.playTime += result.playTime;
            BackendGameData.Instance.GameDataUpdate(); // 서버 저장
        }
        if (autoStartAward) StartCoroutine(AnimateAward());
    }

    int CalculateBonus(GameResultData result)
    {
        float t = result.playTime * Mathf.Max(0, goldPerSecond);
        float k = result.enemyKillCount * Mathf.Max(0, goldPerKill);
        float d = 0f;
        int wl = 0;
        foreach (var w in result.weaponResults)
        {
            d += w.totalDamage * Mathf.Max(0f, goldPerDamage);
            wl += Mathf.Max(0, w.weaponLevel) * Mathf.Max(0, goldPerWeaponLevel);
        }
        int sum = Mathf.RoundToInt(t + k + d + wl);
        return Mathf.Max(0, sum);
    }

    IEnumerator AnimateAward()
    {
        var startDisplay = 0;
        var targetDisplay = startDisplay + bonusAwardGold + Mathf.Max(0, clearBonus);
        float t = 0f;
        while (t < awardAnimDuration)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / awardAnimDuration);
            int cur = Mathf.RoundToInt(Mathf.Lerp(startDisplay, targetDisplay, p));
            goldText.text = "얻은 골드 : " + cur.ToString();
            yield return null;
        }
        goldText.text = "얻은 골드 : " + targetDisplay.ToString();

        int finalWallet = savedGoldBefore + totalAwardGold;
        //if (BackendGameData.Instance != null)
            //BackendGameData.Instance.UserGameData.gold = totalAwardGold;
        //GoldManager.Instance.AddGold(finalWallet); // 서버/로컬 연동
        //PlayerPrefs.SetInt("Gold", finalWallet);
        //PlayerPrefs.Save();
    }

    string FormatTime(float time)
    {
        int min = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);
        return $"{min:D2}:{sec:D2}";
    }

    public void OnClickMainMenu()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ResetRunState();
        SceneManager.LoadScene("MainMenu");
    }

    public void SkipAnimationAndClaim()
    {
        StopAllCoroutines();
        int startDisplay = 0;
        int targetDisplay = startDisplay + bonusAwardGold + Mathf.Max(0, clearBonus);
        goldText.text = "얻은 골드 : " + targetDisplay.ToString();

        int finalWallet = savedGoldBefore + totalAwardGold;
        //GoldManager.Instance.SetGold(finalWallet); // 서버/로컬 연동
        //PlayerPrefs.SetInt("Gold", finalWallet);
        //PlayerPrefs.Save();
    }

    public int GetTotalAwardGold() => totalAwardGold;
    public int GetRunGold() => runAwardGold;
    public int GetBonusGold() => bonusAwardGold;
}

