using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHp = 10;
    float currHp;

    void OnEnable()
    {
        currHp = maxHp;
    }

    public void TakeDamage(float dmg)
    {
        Debug.Log(this.name + " get damage" + dmg);
        currHp -= dmg;
        Debug.Log(this.name + " curr hp is " + currHp);
        if (currHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Dead");
        gameObject.SetActive(false);
    }
}
