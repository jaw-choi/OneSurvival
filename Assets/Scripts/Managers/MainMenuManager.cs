using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    //[SerializeField] private GameObject settingsPanelPrefab;
    //[SerializeField] private Transform uiRoot; // Canvas 아래 빈 오브젝트

    private GameObject settingsInstance;
    void Start()
    {
        if (GameResultData.Instance == null)
        {
            new GameObject("GameResultData").AddComponent<GameResultData>();
        }
        else
        {
            GameResultData.Instance.Reset();
        }
    }

    public void OnClickStart()
    {
        if (GameResultData.Instance != null)
            GameResultData.Instance.Reset();
        SceneManager.LoadScene("GameScene"); // 게임 씬 이름
    }
    public void OnClickUpGrade()
    {
        SceneManager.LoadScene("UpgradeScene");
    }
    public void OnClickSettings()
    {
        //if (settingsInstance == null)
        //    settingsInstance = Instantiate(settingsPanelPrefab, uiRoot);
        //settingsInstance.SetActive(true);
    }

    public void OnClickQuit()
    {
        Debug.Log("Quit Clicked");
        Application.Quit();
    }
    public void OnClickMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
