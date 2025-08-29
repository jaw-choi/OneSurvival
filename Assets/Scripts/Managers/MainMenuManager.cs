using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject settingsPanelPrefab;
    [SerializeField] private Transform uiRoot; // Canvas 아래 빈 오브젝트

    private GameObject settingsInstance;
    void Awake()
    {
        if (startButton) startButton.onClick.AddListener(OnClickStart);
        if (upgradeButton) upgradeButton.onClick.AddListener(OnClickUpGrade);
        if (settingsButton) settingsButton.onClick.AddListener(OnClickSettings);
        if (quitButton) quitButton.onClick.AddListener(OnClickQuit);
    }

    void Start()
    {
        if (GameResultData.Instance == null)
            new GameObject("GameResultData").AddComponent<GameResultData>();
        else
            GameResultData.Instance.Reset();
    }

    public void OnClickStart()
    {
        GameResultData.Instance?.Reset();
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickUpGrade()
    {
        SceneManager.LoadScene("UpgradeScene");
    }

    public void OnClickSettings()
    {
        if (uiRoot == null) uiRoot = FindFirstObjectByType<Canvas>()?.transform; // 안전장치
        if (settingsInstance == null)
            settingsInstance = Instantiate(settingsPanelPrefab, uiRoot);
        settingsInstance.SetActive(true);
    }

    public void OnClickBack()
    {
        if (settingsInstance) settingsInstance.SetActive(false);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }



}
