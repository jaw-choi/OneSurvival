using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    private PlayerExpManager expManager;

    void Start()
    {
        expManager = GetComponent<PlayerExpManager>(); // 또는 싱글톤 접근
        expManager.OnLevelUp += OnLevelUpHandler;
    }

    void OnDestroy()
    {
        expManager.OnLevelUp -= OnLevelUpHandler;
    }

    void OnLevelUpHandler(int level)
    {
        Debug.Log($"레벨 {level} 달성! 무기 선택창 열기");
        Time.timeScale = 0f; // 게임 정지
        WeaponChoiceUI.Instance.ShowChoices(); // 무기 선택 UI 열기
    }
}
