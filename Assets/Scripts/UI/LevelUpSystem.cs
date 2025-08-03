using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    private PlayerExpManager expManager;

    void Start()
    {
        // PlayerExpManager를 GetComponent로 찾거나 싱글톤 접근
        expManager = GetComponent<PlayerExpManager>();

        if (expManager != null)
            expManager.OnLevelUp += OnLevelUpHandler;
        else
            Debug.LogError("PlayerExpManager를 찾을 수 없습니다!");
    }

    void OnDestroy()
    {
        if (expManager != null)
            expManager.OnLevelUp -= OnLevelUpHandler;
    }

    void OnLevelUpHandler(int level)
    {
        Debug.Log($"레벨 {level} 달성! 무기 선택창 열기");

        Time.timeScale = 0f; // 게임 정지

        if (WeaponChoiceUI.Instance != null)
            WeaponChoiceUI.Instance.ShowChoices();
        else
            Debug.LogError("WeaponChoiceUI 인스턴스를 찾을 수 없습니다!");
    }
}
