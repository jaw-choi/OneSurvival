using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    private PlayerExpManager expManager;

    void Start()
    {
        expManager = GetComponent<PlayerExpManager>(); // �Ǵ� �̱��� ����
        expManager.OnLevelUp += OnLevelUpHandler;
    }

    void OnDestroy()
    {
        expManager.OnLevelUp -= OnLevelUpHandler;
    }

    void OnLevelUpHandler(int level)
    {
        Debug.Log($"���� {level} �޼�! ���� ����â ����");
        Time.timeScale = 0f; // ���� ����
        WeaponChoiceUI.Instance.ShowChoices(); // ���� ���� UI ����
    }
}
