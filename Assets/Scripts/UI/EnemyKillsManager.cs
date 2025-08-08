using TMPro;
using UnityEngine;
public class EnemyKillsManager : MonoBehaviour
{
    public TextMeshProUGUI EnemyKillsText;

    void Update()
    {
        EnemyKillsText.text = $"Kills: {EnemyKillCounter.Instance.TotalKills}";
    }

}
