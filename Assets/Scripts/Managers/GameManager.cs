using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public CanvasGroup gameOverPanel;
    public Image fadeImage; // Panel�� Image ������Ʈ
    public TextMeshProUGUI gameOverText;
    private bool readyToClick = false;
    void Awake()
    {
        // �̱��� �ν��Ͻ� �Ҵ�
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameOverPanel.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        Color c = fadeImage.color;
        float duration = 1.5f;
        float time = 0;

        // start from UI hide
        gameOverText.alpha = 0;

        while (time < duration)
        {
            float t = time / duration;
            fadeImage.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0, 0.6f, t));
            gameOverPanel.alpha = Mathf.Lerp(0f, 1f, t);
            gameOverText.alpha = Mathf.Lerp(0, 1, t);
            time += Time.deltaTime;
            yield return null;
        }

        // fix final alpha value
        fadeImage.color = new Color(c.r, c.g, c.b, 0.6f);
        gameOverText.alpha = 1;
        readyToClick = true;
    }
    private void Update()
    {
        if (readyToClick && Input.GetMouseButtonDown(0))
        {
            // Ŭ���ϸ� �� ��ȯ
            SceneManager.LoadScene("ResultScene");
        }
    }
}