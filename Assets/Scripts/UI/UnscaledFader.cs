using System;
using UnityEngine;
using System.Collections;

public class UnscaledFader
{
    MonoBehaviour host;
    public UnscaledFader()
    {
        var go = GameObject.Find("__UnscaledFaderHost__");
        if (go == null) go = new GameObject("__UnscaledFaderHost__");
        if (go.GetComponent<FaderHost>() == null) go.AddComponent<FaderHost>();
        host = go.GetComponent<FaderHost>();
    }

    public void Fade(CanvasGroup cg, float from, float to, float duration, Action onDone = null)
    {
        host.StartCoroutine(FadeRoutine(cg, from, to, duration, onDone));
    }

    IEnumerator FadeRoutine(CanvasGroup cg, float from, float to, float duration, Action onDone)
    {
        float t = 0f;
        cg.alpha = from;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / duration);
            cg.alpha = Mathf.Lerp(from, to, k);
            yield return null;
        }
        cg.alpha = to;
        onDone?.Invoke();
    }

    class FaderHost : MonoBehaviour { }
}
