using UnityEngine;

public class Reposition : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;
        if (GameManager.Instance.IsGameOver)
            return;

        Vector3 playerPos = GameManager.Instance.PlayerTransform.position;
        Vector3 myPos = transform.position;



        switch (transform.tag)
        {
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 48);
                }
                else if(diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 48);
                }
                break;
            case "Enemy":
                break;
        }
    }
}
