using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameOverState : IGameState
{
    private CanvasGroup panel;
    private Image fadeImage;
    private TextMeshProUGUI gameOverText;
    private bool readyToClick = false;

    public void Enter()
    {
        Debug.Log("GameOver 상태 진입");

        panel = GameManager.Instance.gameOverPanel;
        fadeImage = GameManager.Instance.fadeImage;
        gameOverText = GameManager.Instance.gameOverText;

        panel.gameObject.SetActive(true);
        GameManager.Instance.StartCoroutine(FadeIn());
    }

    public void Update()
    {
        if (readyToClick && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("ResultScene");
        }
    }

    public void Exit()
    {
        Debug.Log("GameOver 상태 종료");
    }

    private IEnumerator FadeIn()
    {
        Color c = fadeImage.color;
        float duration = 1.5f;
        float time = 0;

        gameOverText.alpha = 0;

        while (time < duration)
        {
            float t = time / duration;
            fadeImage.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0, 0.6f, t));
            panel.alpha = Mathf.Lerp(0f, 1f, t);
            gameOverText.alpha = Mathf.Lerp(0, 1, t);
            time += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, 0.6f);
        gameOverText.alpha = 1;
        readyToClick = true;
    }
}
