using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Progress : MonoBehaviour
{
    [SerializeField] private Slider sliderProgress;
    [SerializeField] private TextMeshProUGUI textProgressData;
    [SerializeField] private float fakeLoadingTime = 1f; // 최소 가짜 로딩 시간 (초)

    public void Play(string sceneName, UnityAction action = null)
    {
        StartCoroutine(OnProgress(sceneName, action));
    }

    private IEnumerator OnProgress(string sceneName, UnityAction action)
    {
        // ---------- ① 가짜 로딩 단계 ----------
        float elapsed = 0f;
        while (elapsed < fakeLoadingTime)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / fakeLoadingTime;
            float eased = 1 - Mathf.Pow(1 - percent, 3); // EaseOutCubic
            sliderProgress.value = Mathf.Lerp(0, 0.99f, eased);

            textProgressData.text = $"Now Loading... {sliderProgress.value * 100:F0}%";
            yield return null;
        }

        // ---------- ② 진짜 로딩 단계 ----------
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            float realProgress = Mathf.Clamp01(asyncOp.progress / 0.9f);
            float combined = Mathf.Lerp(0.99f, 1f, realProgress); // 나머지 20% 구간은 실제 로딩 반영

            sliderProgress.value = combined;
            textProgressData.text = $"Now Loading... {combined * 100:F0}%";

            // 진짜 로딩 완료 시
            if (asyncOp.progress >= 0.9f)
            {
                textProgressData.text = "Loading Complete!";
                yield return new WaitForSeconds(0.5f);
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }

        action?.Invoke();
    }
}
