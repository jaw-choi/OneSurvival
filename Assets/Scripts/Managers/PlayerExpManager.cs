using UnityEngine;

public class PlayerExpManager : MonoBehaviour
{
    public static PlayerExpManager Instance { get; private set; }
    [Header("경험치 상태")]
    public int currentExp = 0;
    public int currentLevel = 1;
    public int expToNextLevel = 10;

    [Header("레벨업 증가량")]
    public float expGrowthRate = 2.2f; // 다음 레벨업까지 필요 경험치 증가 비율

    public System.Action<int> OnLevelUp; // 레벨업 이벤트 (UI나 이펙트 연결용)

    void Awake()
    {
        Instance = this;
    }
    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"EXP +{amount} → {currentExp}/{expToNextLevel}");

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            currentLevel++;

            expToNextLevel = Mathf.RoundToInt(expToNextLevel * expGrowthRate);
            Debug.Log($"레벨업! 현재 레벨: {currentLevel}, 다음 레벨 EXP: {expToNextLevel}");

            OnLevelUp?.Invoke(currentLevel); // UI/스킬 선택 등과 연동 가능
        }
    }
}
