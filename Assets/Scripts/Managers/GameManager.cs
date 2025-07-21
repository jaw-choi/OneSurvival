using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameOver { get; private set; } = false;
    public Transform PlayerTransform { get; private set; }
    public CanvasGroup gameOverPanel;
    public Image fadeImage; // Panel�� Image ������Ʈ
    public TextMeshProUGUI gameOverText;
    private bool readyToClick = false;
    public int Gold { get; private set; } = 0;
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
        IsGameOver = false;
    }

    void Start()
    {
        gameOverPanel.gameObject.SetActive(false);
        PlayerTransform = FindAnyObjectByType<PlayerMovement>().transform;
    }

    public void GameOver()
    {
        IsGameOver = true;
        //Time.timeScale = 0f;
        gameOverPanel.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }
    public void AddGold(int value)
    {
        Gold += value;
        // ��尡 �ٲ�� UI ������Ʈ
        UIManager.Instance.UpdateGoldUI(Gold);
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