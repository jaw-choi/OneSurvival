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
            GameResultData.Instance.Reset(); // ���� ��� ����
        }
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("GameScene"); // ���� �� �̸�
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
