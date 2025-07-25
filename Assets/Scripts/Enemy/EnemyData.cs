using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float maxHP;
    public float moveSpeed;
    public int expDrop;
    public GameObject expGemPrefab;
    public RuntimeAnimatorController animatorController;
    public Sprite enemySprite;

    // 추가 가능: 공격력, 공격 딜레이, 사운드 등
}
