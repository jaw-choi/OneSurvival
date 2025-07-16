using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHp = 100;
    int currHp;

    void Start()
    {
        currHp = maxHp;
    }

    public void TakeDamage(int dmg)
    {
        currHp -= dmg;
        if (currHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Dead");
        GameManager.Instance.GameOver();
        gameObject.SetActive(false);
    }
}
