using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;                       //   플레이어 트랜스폼
    public GameObject chunkPrefab;                 //   청크 프리팹(바닥 타일·소품 부모 등 포함)
    public int gridRadius = 1;                     //   1 => 3x3 유지, 2 => 5x5
    public int chunkSize = 32;                     //   월드 유닛 기준 청크 한 변

    [Header("Procedural")]
    public int worldSeed = 12345;                  //   결정적 생성용 시드

    //   내부 상태
    private Vector2Int currentCenter;              //   현재 플레이어가 속한 청크 좌표
    private readonly Dictionary<Vector2Int, Chunk> live = new();   //   활성 청크 맵
    private readonly Queue<Chunk> pool = new();                    //   청크 객체 풀

    void Start()
    {
        //   초기 활성 청크 생성
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

    //   플레이어 주변 링 유지
    private void RefreshActiveRing()
    {
        //   유지해야 할 좌표 집합
        HashSet<Vector2Int> needed = new();
        for (int y = -gridRadius; y <= gridRadius; y++)
            for (int x = -gridRadius; x <= gridRadius; x++)
                needed.Add(new Vector2Int(currentCenter.x + x, currentCenter.y + y));

        //   불필요 청크를 후보로 모음
        List<Vector2Int> toRecycle = new();
        foreach (var kv in live)
            if (!needed.Contains(kv.Key)) toRecycle.Add(kv.Key);

        //   재배치: 필요 좌표 중 미보유 좌표에 기존 청크를 이동
        foreach (var key in toRecycle)
        {
            var ch = live[key];
            live.Remove(key);

            //   가장 가까운 미보유 좌표 하나를 찾아 이동
            Vector2Int target = FindNearestMissing(needed);
            if (target != Vector2Int.zero)
            {
                live[target] = ch;
                PositionAndRebuild(ch, target);
                needed.Remove(target);
            }
            else
            {
                //   예외적으로 풀에 반환
                ch.gameObject.SetActive(false);
                pool.Enqueue(ch);
            }
        }

        //   남은 필요한 좌표는 새로 꺼내거나 생성
        foreach (var missing in needed)
            SpawnOrMoveChunk(missing);
    }

    private Vector2Int FindNearestMissing(HashSet<Vector2Int> needed)
    {
        //   간단 구현: 집합의 임의 원소 반환
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
        //   월드 위치 배치
        Vector3 worldPos = new Vector3(cpos.x * chunkSize, cpos.y * chunkSize, 0f);
        ch.transform.position = worldPos;

        //   내용 재생성(결정적)
        int seed = Hash(worldSeed, cpos.x, cpos.y);
        ch.Rebuild(chunkSize, seed);
    }

    private Vector2Int WorldToChunk(Vector3 world)
    {
        int cx = Mathf.FloorToInt(world.x / chunkSize);
        int cy = Mathf.FloorToInt(world.y / chunkSize);
        return new Vector2Int(cx, cy);
    }

    //   단순 해시
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
