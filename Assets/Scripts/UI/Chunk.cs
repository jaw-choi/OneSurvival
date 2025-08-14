using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    public Tilemap groundTilemap;
    public TileBase groundTile;

    [Tooltip("자식에서 Tilemap을 자동으로 찾아 바인딩")]
    public bool autoBind = true;

    void Reset()
    {
        // 에디터에서 붙였을 때 자동 바인딩
        if (autoBind && !groundTilemap)
            groundTilemap = GetComponentInChildren<Tilemap>();
    }

    void Awake()
    {
        if (autoBind && !groundTilemap)
            groundTilemap = GetComponentInChildren<Tilemap>();
    }

    void OnValidate()
    {
        if (autoBind && !groundTilemap)
            groundTilemap = GetComponentInChildren<Tilemap>();
    }
    public void Rebuild(int size, int seed)
    {
        if (!groundTilemap || !groundTile)
        {
            Debug.LogWarning("[Chunk] Tilemap 또는 TileBase 미지정", this);
            return;
        }

        groundTilemap.ClearAllTiles();
        var rand = new System.Random(seed);

        for (int x = -size / 2; x < size / 2; x++)
        {
            for (int y = -size / 2; y < size / 2; y++)
            {
                groundTilemap.SetTile(new Vector3Int(x, y, 0), groundTile);
            }
        }
    }
}