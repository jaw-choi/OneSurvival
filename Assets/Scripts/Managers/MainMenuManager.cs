using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    //[SerializeField] private GameObject settingsPanelPrefab;
    //[SerializeField] private Transform uiRoot; // Canvas �Ʒ� �� ������Ʈ

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
        SceneManager.LoadScene("GameScene"); // ���� �� �̸�
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
