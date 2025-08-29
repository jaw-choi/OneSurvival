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
    private Button quitButton;
    private bool readyToClick = false;

    public void Enter()
    {
        panel = GameManager.Instance.gameOverPanel;
        fadeImage = GameManager.Instance.fadeImage;
        gameOverText = GameManager.Instance.gameOverText;
        quitButton = GameManager.Instance.quitButton;

        panel.gameObject.SetActive(true);

        // �����ʴ� Enter �� '�� �� ��' ���
        if (quitButton != null)
        {
            quitButton.onClick.RemoveListener(OnClickQuit); // �ߺ� ����
            quitButton.onClick.AddListener(OnClickQuit);
            quitButton.interactable = false;                 // ���� ���� ������ ���
        }

        GameManager.Instance.StartCoroutine(FadeIn());
    }

    // �� �̻� �� ������ �� �� ����
    public void Update() { }

    public void Exit()
    {
        // ���� ���� �� ����
        if (quitButton != null)
        {
            quitButton.onClick.RemoveListener(OnClickQuit);
            quitButton.interactable = false;
        }
    }

    private void OnClickQuit()
    {
        if (!readyToClick) return;       // ������ġ
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("ResultScene");
    }

    private IEnumerator FadeIn()
    {
        Color c = fadeImage.color;
        float duration = 1.5f;
        float time = 0f;

        gameOverText.alpha = 0f;

        while (time < duration)
        {
            float t = time / duration;
            fadeImage.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0f, 0.6f, t));
            panel.alpha = Mathf.Lerp(0f, 1f, t);
            gameOverText.alpha = Mathf.Lerp(0f, 1f, t);
            time += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, 0.6f);
        gameOverText.alpha = 1f;

        readyToClick = true;
        if (quitButton != null) quitButton.interactable = true; // ���� Ŭ�� ���
    }
}
