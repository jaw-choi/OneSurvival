using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public CanvasGroup pausePanel;
    //public CanvasGroup overlay;
    public float fadeDuration = 0.15f;

    bool isPaused;
    UnscaledFader fader;

    void Awake()
    {
        fader = new UnscaledFader();
        SetPanel(false);
    }

    public void Toggle()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        SetPanel(true);
        fader.Fade(pausePanel, 0f, 1f, fadeDuration);
        //fader.Fade(overlay, 0f, 1f, fadeDuration);
    }

    public void Resume()
    {
        if (!isPaused) return;
        isPaused = false;
        fader.Fade(pausePanel, 1f, 0f, fadeDuration, () =>
        {
            SetPanel(false);
            Time.timeScale = 1f;
            AudioListener.pause = false;
        });
        //fader.Fade(overlay, 1f, 0f, fadeDuration);
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

    void SetPanel(bool show)
    {
        pausePanel.alpha = show ? 1f : 0f;
        pausePanel.interactable = show;
        pausePanel.blocksRaycasts = show;
        //overlay.alpha = show ? 1f : 0f;
        //overlay.interactable = show;
        //overlay.blocksRaycasts = show;
    }
}
