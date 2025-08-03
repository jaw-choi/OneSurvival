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

    private List<WeaponData> allWeaponData;
    private List<WeaponData> currentChoices;

    void Awake()
    {
        Instance = this;
        allWeaponData = WeaponDatabaseLoader.Instance.GetAll(); // 무기 데이터 리스트 가져오기
        
    }

    public void ShowChoices()
    {
        panel.SetActive(true);
        currentChoices = GetRandomWeaponChoices(3);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i;
            WeaponData weapon = currentChoices[i];

            choiceTexts[i].text = weapon.weaponName;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnWeaponSelected(index));
        }
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

        panel.SetActive(false);
        Time.timeScale = 1f; // 시간 재개
    }

}
