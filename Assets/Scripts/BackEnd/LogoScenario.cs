using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScenario : MonoBehaviour
{
    [SerializeField] private Progress progress;
    private void Awake()
    {
        SystemSetup();
    }
    private void SystemSetup()
    {
        Application.runInBackground = true;

        int width = Screen.width;
        int height = (int)(Screen.width * 16f/9);
        Screen.SetResolution(width, height, true);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        progress.Play(OnAfterProgress);
    }
    private void OnAfterProgress()
    {
        SceneManager.LoadScene("Login");
    }
}
