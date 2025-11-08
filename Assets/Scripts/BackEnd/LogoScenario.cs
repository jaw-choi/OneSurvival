using UnityEngine;

public class LogoScenario : MonoBehaviour
{
    [SerializeField] private Progress progress;

    private void Awake()
    {
        SystemSetup();
    }

    private void SystemSetup()
    {
        // 시스템 설정
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int width = Screen.width;
        int height = (int)(Screen.width * 16f / 9);
        Screen.SetResolution(width, height, true);

        // 이제 MainMenu 로딩을 Progress가 실제로 처리
        progress.Play("MainMenu", OnAfterProgress);
    }

    private void OnAfterProgress()
    {
        Debug.Log("MainMenu Scene successfully loaded!");
    }
}
