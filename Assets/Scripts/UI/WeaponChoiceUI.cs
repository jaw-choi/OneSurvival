using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponChoiceUI : MonoBehaviour
{
    public static WeaponChoiceUI Instance;

    public GameObject panel;
    public Button[] choiceButtons;
    public TextMeshProUGUI[] choiceTexts;

    // ==== 추가 시작 ====
    // 각 선택지의 아이콘, 레벨, 설명 표시용 UI 참조
    public Image[] choiceIcons;
    public TextMeshProUGUI[] choiceLevelTexts;
    public TextMeshProUGUI[] choiceDescTexts;
    // ==== 추가 끝 ====

    private List<WeaponData> allWeaponData;
    private List<WeaponData> currentChoices;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        allWeaponData = WeaponDatabaseLoader.Instance.GetAll();
    }

    public void ShowChoices()
    {
        panel.SetActive(true);
        currentChoices = GetRandomWeaponChoices(3);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBGM(true);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i;
            WeaponData weapon = currentChoices[i];

            // 기존: 이름
            choiceTexts[i].text = weapon.weaponName;

            // 아이콘
            if (choiceIcons != null && i < choiceIcons.Length && choiceIcons[i] != null)
                choiceIcons[i].sprite = weapon.weaponIcon;

            // 레벨: 무기를 보유 중이면 현재 레벨, 아니면 '신규'
            if (choiceLevelTexts != null && i < choiceLevelTexts.Length && choiceLevelTexts[i] != null)
            {
                bool has = WeaponManager.Instance.HasWeapon(weapon);
                int level = weapon.currentLevel;
                choiceLevelTexts[i].text = has ? $"Lv.{level}" : "신규";
            }

            // 설명: SO에 짧은 설명 필드가 있다고 가정(없으면 이름을 대신 사용)
            //if (choiceDescTexts != null && i < choiceDescTexts.Length && choiceDescTexts[i] != null)
            //{
            //    string desc = !string.IsNullOrEmpty(weapon.shortDescription) ? weapon.shortDescription : weapon.weaponName;
            //    choiceDescTexts[i].text = desc;
            //}
            // ==== 추가 끝 ====

            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnWeaponSelected(index));
        }

        // 필요 시 레벨업 동안 정지
        // ==== 추가 시작 ====
        // Time.timeScale = 0f;
        // ==== 추가 끝 ====
    }

    private List<WeaponData> GetRandomWeaponChoices(int count)
    {
        List<WeaponData> pool = new List<WeaponData>(allWeaponData);
        List<WeaponData> result = new List<WeaponData>();

        while (result.Count < count && pool.Count > 0)
        {
            int rand = Random.Range(0, pool.Count);
            result.Add(pool[rand]);
            pool.RemoveAt(rand);
        }

        return result;
    }

    private void OnWeaponSelected(int index)
    {
        WeaponData selected = currentChoices[index];

        if (WeaponManager.Instance.HasWeapon(selected))
        {
            WeaponManager.Instance.UpgradeWeapon(selected);
        }
        else
        {
            WeaponManager.Instance.AddWeapon(selected);
        }
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBGM(false);
        panel.SetActive(false);
        Time.timeScale = 1f; // 시간 재개
    }

    // ==== 추가 시작 ====
    // 무기 현재 레벨을 WeaponManager에서 가져오되,
    // GetWeaponLevel(WeaponData) 메서드가 없는 프로젝트에서도 빌드되도록 리플렉션으로 안전 호출
    private int? TryGetWeaponLevel(WeaponData weapon)
    {
        var wm = WeaponManager.Instance;
        if (wm == null) return null;

        var mi = wm.GetType().GetMethod("GetWeaponLevel", new System.Type[] { typeof(WeaponData) });
        if (mi != null)
        {
            object ret = mi.Invoke(wm, new object[] { weapon });
            if (ret is int lv) return lv;
        }
        return null;
    }
    // ==== 추가 끝 ====
}
