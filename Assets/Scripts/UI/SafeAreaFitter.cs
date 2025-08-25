using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    RectTransform rt;
    Rect lastSafe;
    Vector2Int lastRes;
    ScreenOrientation lastOri;

    void Awake() => rt = GetComponent<RectTransform>();
    void OnEnable() => Apply();
    void Update()
    {
        if (Screen.safeArea != lastSafe ||
            new Vector2Int(Screen.width, Screen.height) != lastRes ||
            Screen.orientation != lastOri)
            Apply();
    }
    void Apply()
    {
        var pixelSafe = Screen.safeArea;
        lastSafe = pixelSafe;
        lastRes  = new Vector2Int(Screen.width, Screen.height);
        lastOri  = Screen.orientation;

        // Canvas 기준으로 정규화
        var anchorMin = pixelSafe.position;
        var anchorMax = pixelSafe.position + pixelSafe.size;
        anchorMin.x /= Screen.width;  anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;  anchorMax.y /= Screen.height;
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
