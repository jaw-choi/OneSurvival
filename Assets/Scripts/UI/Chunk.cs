using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    public Tilemap groundTilemap;
    public TileBase groundTile;

    [Tooltip("�ڽĿ��� Tilemap�� �ڵ����� ã�� ���ε�")]
    public bool autoBind = true;

    void Reset()
    {
        // �����Ϳ��� �ٿ��� �� �ڵ� ���ε�
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
            Debug.LogWarning("[Chunk] Tilemap �Ǵ� TileBase ������", this);
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