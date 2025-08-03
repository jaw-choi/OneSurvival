using UnityEngine;

public class PlayerExpManager : MonoBehaviour
{
    public static PlayerExpManager Instance { get; private set; }
    [Header("����ġ ����")]
    public int currentExp = 0;
    public int currentLevel = 1;
    public int expToNextLevel = 10;

    [Header("������ ������")]
    public float expGrowthRate = 2.2f; // ���� ���������� �ʿ� ����ġ ���� ����

    public System.Action<int> OnLevelUp; // ������ �̺�Ʈ (UI�� ����Ʈ �����)

    void Awake()
    {
        Instance = this;
    }
    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"EXP +{amount} �� {currentExp}/{expToNextLevel}");

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            currentLevel++;

            expToNextLevel = Mathf.RoundToInt(expToNextLevel * expGrowthRate);
            Debug.Log($"������! ���� ����: {currentLevel}, ���� ���� EXP: {expToNextLevel}");

            OnLevelUp?.Invoke(currentLevel); // UI/��ų ���� ��� ���� ����
        }
    }
}
