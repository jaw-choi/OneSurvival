using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    private PlayerExpManager expManager;

    void Start()
    {
        // PlayerExpManager�� GetComponent�� ã�ų� �̱��� ����
        expManager = GetComponent<PlayerExpManager>();

        if (expManager != null)
            expManager.OnLevelUp += OnLevelUpHandler;
        else
            Debug.LogError("PlayerExpManager�� ã�� �� �����ϴ�!");
    }

    void OnDestroy()
    {
        if (expManager != null)
            expManager.OnLevelUp -= OnLevelUpHandler;
    }

    void OnLevelUpHandler(int level)
    {
        Debug.Log($"���� {level} �޼�! ���� ����â ����");

        Time.timeScale = 0f; // ���� ����

        if (WeaponChoiceUI.Instance != null)
            WeaponChoiceUI.Instance.ShowChoices();
        else
            Debug.LogError("WeaponChoiceUI �ν��Ͻ��� ã�� �� �����ϴ�!");
    }
}
