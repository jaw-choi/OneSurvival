using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayTimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI playTimeText; // ¶Ç´Â public Text playTimeText;
    private float elapsedTime = 0f;

    void Update()
    {
        if(GameManager.Instance.IsGameOver) return;
        elapsedTime += Time.deltaTime;

        //int hours = (int)(elapsedTime / 3600);
        int minutes = (int)(elapsedTime % 3600 / 60);
        int seconds = (int)(elapsedTime % 60);

        playTimeText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }
    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
