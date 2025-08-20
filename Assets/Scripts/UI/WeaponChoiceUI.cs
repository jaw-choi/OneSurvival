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

    // ==== �߰� ���� ====
    // �� �������� ������, ����, ���� ǥ�ÿ� UI ����
    public Image[] choiceIcons;
    public TextMeshProUGUI[] choiceLevelTexts;
    public TextMeshProUGUI[] choiceDescTexts;
    // ==== �߰� �� ====

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

            // ����: �̸�
            choiceTexts[i].text = weapon.weaponName;

            // ������
            if (choiceIcons != null && i < choiceIcons.Length && choiceIcons[i] != null)
                choiceIcons[i].sprite = weapon.weaponIcon;

            // ����: ���⸦ ���� ���̸� ���� ����, �ƴϸ� '�ű�'
            if (choiceLevelTexts != null && i < choiceLevelTexts.Length && choiceLevelTexts[i] != null)
            {
                bool has = WeaponManager.Instance.HasWeapon(weapon);
                int level = weapon.currentLevel;
                choiceLevelTexts[i].text = has ? $"Lv.{level}" : "�ű�";
            }

            // ����: SO�� ª�� ���� �ʵ尡 �ִٰ� ����(������ �̸��� ��� ���)
            //if (choiceDescTexts != null && i < choiceDescTexts.Length && choiceDescTexts[i] != null)
            //{
            //    string desc = !string.IsNullOrEmpty(weapon.shortDescription) ? weapon.shortDescription : weapon.weaponName;
            //    choiceDescTexts[i].text = desc;
            //}
            // ==== �߰� �� ====

            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnWeaponSelected(index));
        }

        // �ʿ� �� ������ ���� ����
        // ==== �߰� ���� ====
        // Time.timeScale = 0f;
        // ==== �߰� �� ====
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
        Time.timeScale = 1f; // �ð� �簳
    }

    // ==== �߰� ���� ====
    // ���� ���� ������ WeaponManager���� ��������,
    // GetWeaponLevel(WeaponData) �޼��尡 ���� ������Ʈ������ ����ǵ��� ���÷������� ���� ȣ��
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
    // ==== �߰� �� ====
}
