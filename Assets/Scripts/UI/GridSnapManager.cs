using UnityEngine;

public class GridSnapManager : MonoBehaviour
{
    // 2×2 예시: [BL, BR, TL, TR]
    [SerializeField] Transform player;
    [SerializeField] Transform[] chunks;
    [SerializeField] float stepX = 24f, stepY = 24f;
    [SerializeField] Vector2 anchor = Vector2.zero;

    Vector2Int lastIdx = new Vector2Int(int.MinValue, int.MinValue);

    void LateUpdate()
    {
        if (!player) { enabled = false; return; }
        var p = player.position;
        int ix = Mathf.FloorToInt((p.x - anchor.x) / stepX);
        int iy = Mathf.FloorToInt((p.y - anchor.y) / stepY);

        if (ix == lastIdx.x && iy == lastIdx.y) return; // 대부분 프레임은 여기서 끝
        lastIdx.x = ix; lastIdx.y = iy;

        float blx = anchor.x + ix * stepX;
        float bly = anchor.y + iy * stepY;

        if (chunks[0]) chunks[0].position = new Vector3(blx, bly, chunks[0].position.z); // BL
        if (chunks[1]) chunks[1].position = new Vector3(blx + stepX, bly, chunks[1].position.z); // BR
        if (chunks[2]) chunks[2].position = new Vector3(blx, bly + stepY, chunks[2].position.z); // TL
        if (chunks[3]) chunks[3].position = new Vector3(blx + stepX, bly + stepY, chunks[3].position.z); // TR
    }

}
