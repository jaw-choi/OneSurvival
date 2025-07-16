using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("GameScene"); // ���� �� �̸�
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
