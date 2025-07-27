using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIExpDisplay : MonoBehaviour
{
    public TextMeshProUGUI expText; // 현재 경험치를 표시할 Text
    public Slider expBar; // (선택) 경험치 바
    private PlayerExpManager playerExp;

    void Start()
    {
        playerExp = FindFirstObjectByType<PlayerExpManager>();
    }

    void Update()
    {
        if (playerExp != null)
        {
            expText.text = "LV" + playerExp.currentLevel.ToString();

            if (expBar != null)
            {
                expBar.maxValue = playerExp.expToNextLevel;
                expBar.value = playerExp.currentExp;    
            }
        }
    }
}
