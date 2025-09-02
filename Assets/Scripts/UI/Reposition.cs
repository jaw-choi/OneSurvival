using UnityEngine;

public class Reposition : MonoBehaviour
{
    //¹ö±× ÀÌ½´·Î Æó±â
    int tileSize = 48;
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
                //if(diffX >24f && diffY > 24f)
                //{
                //    //transform.Translate(Vector3.right * dirX * tileSize);
                //    //transform.Translate(Vector3.up * dirY * tileSize);
                //    transform.Translate(new Vector3(dirX * tileSize, dirY * tileSize, 0));
                //    Debug.Log(name + " XX YY  " + "diffX: " + diffX + " diffY: " + diffY);

                //}            
                //else
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * tileSize);
                    Debug.Log(name + " XX  " + "diffX: " + diffX + " diffY: " + diffY);
                }
                else if(diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * tileSize);
                    Debug.Log(name + " YY  " + "diffX: " + diffX + " diffY: " + diffY);
                }
                break;
            case "Enemy":
                break;
        }
    }
}
