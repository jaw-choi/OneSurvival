using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{

    public float fadeDuration = 0.15f;

    bool isPaused;
    UnscaledFader fader;

    [SerializeField] private GameObject settingsPanelPrefab;
    [SerializeField] private Transform uiRoot; // Canvas 아래 빈 오브젝트

    [SerializeField] private string mainMenuSceneName = "MainMenu";
    private GameObject settingsInstance;

    void Awake()
    {
        fader = new UnscaledFader();
    }

    public void Toggle()
    {
        Pause();
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;

        if (uiRoot == null) uiRoot = FindFirstObjectByType<Canvas>()?.transform; // 안전장치
        if (settingsInstance == null)
            settingsInstance = Instantiate(settingsPanelPrefab, uiRoot);
        settingsInstance.SetActive(true);
        var panel = settingsInstance.GetComponent<SettingsUI>();
        panel.SetMode(SettingsUI.SettingsMode.InGame);

        var mainMenuBtn = settingsInstance.transform.Find("Button_MainMenu")?.GetComponent<Button>();
        if (mainMenuBtn != null)
        {
            mainMenuBtn.gameObject.SetActive(true);
            mainMenuBtn.onClick.RemoveAllListeners();
            mainMenuBtn.onClick.AddListener(GoToMainMenu);

        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Resume()
    {
        if (settingsInstance)
            settingsInstance.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }


    public void Restart()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}


