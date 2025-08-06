using UnityEngine;

public class EnemyKillCounter : MonoBehaviour
{
    public static EnemyKillCounter Instance { get; private set; }

    public int TotalKills { get; private set; } = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddKill()
    {
        TotalKills++;
    }
}
