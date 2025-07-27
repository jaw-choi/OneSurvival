using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIExpDisplay : MonoBehaviour
{
    public TextMeshProUGUI expText; // ���� ����ġ�� ǥ���� Text
    public Slider expBar; // (����) ����ġ ��
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
