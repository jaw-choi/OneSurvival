using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        if (GameResultData.Instance == null)
        {
            new GameObject("GameResultData").AddComponent<GameResultData>();
        }
        else
        {
            GameResultData.Instance.Reset(); // 이전 기록 제거
        }
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("GameScene"); // 게임 씬 이름
    }
    public void OnClickUpGrade()
    {
        SceneManager.LoadScene("UpgradeScene");
    }
    public void OnClickSettings()
    {
        Debug.Log("Settings clicked");
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
