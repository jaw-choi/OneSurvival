using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;                       //   �÷��̾� Ʈ������
    public GameObject chunkPrefab;                 //   ûũ ������(�ٴ� Ÿ�ϡ���ǰ �θ� �� ����)
    public int gridRadius = 1;                     //   1 => 3x3 ����, 2 => 5x5
    public int chunkSize = 32;                     //   ���� ���� ���� ûũ �� ��

    [Header("Procedural")]
    public int worldSeed = 12345;                  //   ������ ������ �õ�

    //   ���� ����
    private Vector2Int currentCenter;              //   ���� �÷��̾ ���� ûũ ��ǥ
    private readonly Dictionary<Vector2Int, Chunk> live = new();   //   Ȱ�� ûũ ��
    private readonly Queue<Chunk> pool = new();                    //   ûũ ��ü Ǯ

    void Start()
    {
        //   �ʱ� Ȱ�� ûũ ����
        currentCenter = WorldToChunk(player.position);
        for (int y = -gridRadius; y <= gridRadius; y++)
            for (int x = -gridRadius; x <= gridRadius; x++)
            {
                var cpos = new Vector2Int(currentCenter.x + x, currentCenter.y + y);
                SpawnOrMoveChunk(cpos);
            }
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
            Vector2Int pChunk = WorldToChunk(player.position);
        if (pChunk != currentCenter)
        {
            currentCenter = pChunk;
            RefreshActiveRing();
        }
    }

    //   �÷��̾� �ֺ� �� ����
    private void RefreshActiveRing()
    {
        //   �����ؾ� �� ��ǥ ����
        HashSet<Vector2Int> needed = new();
        for (int y = -gridRadius; y <= gridRadius; y++)
            for (int x = -gridRadius; x <= gridRadius; x++)
                needed.Add(new Vector2Int(currentCenter.x + x, currentCenter.y + y));

        //   ���ʿ� ûũ�� �ĺ��� ����
        List<Vector2Int> toRecycle = new();
        foreach (var kv in live)
            if (!needed.Contains(kv.Key)) toRecycle.Add(kv.Key);

        //   ���ġ: �ʿ� ��ǥ �� �̺��� ��ǥ�� ���� ûũ�� �̵�
        foreach (var key in toRecycle)
        {
            var ch = live[key];
            live.Remove(key);

            //   ���� ����� �̺��� ��ǥ �ϳ��� ã�� �̵�
            Vector2Int target = FindNearestMissing(needed);
            if (target != Vector2Int.zero)
            {
                live[target] = ch;
                PositionAndRebuild(ch, target);
                needed.Remove(target);
            }
            else
            {
                //   ���������� Ǯ�� ��ȯ
                ch.gameObject.SetActive(false);
                pool.Enqueue(ch);
            }
        }

        //   ���� �ʿ��� ��ǥ�� ���� �����ų� ����
        foreach (var missing in needed)
            SpawnOrMoveChunk(missing);
    }

    private Vector2Int FindNearestMissing(HashSet<Vector2Int> needed)
    {
        //   ���� ����: ������ ���� ���� ��ȯ
        foreach (var v in needed) return v;
        return Vector2Int.zero;
    }

    private void SpawnOrMoveChunk(Vector2Int cpos)
    {
        if (live.ContainsKey(cpos)) return;

        Chunk ch = null;
        if (pool.Count > 0)
        {
            ch = pool.Dequeue();
            ch.gameObject.SetActive(true);
        }
        else
        {
            var go = Instantiate(chunkPrefab);
            ch = go.GetComponent<Chunk>();
            if (!ch) ch = go.AddComponent<Chunk>();
        }
        live[cpos] = ch;
        PositionAndRebuild(ch, cpos);
    }

    private void PositionAndRebuild(Chunk ch, Vector2Int cpos)
    {
        //   ���� ��ġ ��ġ
        Vector3 worldPos = new Vector3(cpos.x * chunkSize, cpos.y * chunkSize, 0f);
        ch.transform.position = worldPos;

        //   ���� �����(������)
        int seed = Hash(worldSeed, cpos.x, cpos.y);
        ch.Rebuild(chunkSize, seed);
    }

    private Vector2Int WorldToChunk(Vector3 world)
    {
        int cx = Mathf.FloorToInt(world.x / chunkSize);
        int cy = Mathf.FloorToInt(world.y / chunkSize);
        return new Vector2Int(cx, cy);
    }

    //   �ܼ� �ؽ�
    private int Hash(int a, int b, int c)
    {
        unchecked
        {
            int h = a;
            h = h * 73856093 ^ b;
            h = h * 19349663 ^ c;
            return h;
        }
    }
}
