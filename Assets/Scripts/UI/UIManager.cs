using UnityEngine;
using TMPro; // TextMeshPro »ç¿ë ½Ã

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TextMeshProUGUI goldText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateGoldUI(int gold)
    {
        goldText.text = "Gold : " + gold;
    }

}
