using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("GameScene"); // ∞‘¿” æ¿ ¿Ã∏ß
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
}
