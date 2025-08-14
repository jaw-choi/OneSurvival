using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner_TimeProgressive : MonoBehaviour
{
    public EnemyPoolHub poolHub;
    public SpawnProgressionData progression;
    public FormationType formation = FormationType.Arc;

    float groupTimer;

    void Start()
    {
        if (progression != null)
            progression.windows.Sort((a, b) => a.startTime.CompareTo(b.startTime));
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsGameOver || progression == null)
            return;

        float elapsed = GameManager.Instance.ElapsedTime;

        var weighted = BuildActiveWeights(elapsed);
        if (weighted.Count == 0) return;

        float groupInterval = Mathf.Max(0.05f, progression.groupIntervalSec.Evaluate(elapsed));
        int groupSize = Mathf.Max(1, Mathf.RoundToInt(progression.groupSize.Evaluate(elapsed)));

        groupTimer += Time.deltaTime;
        if (groupTimer >= groupInterval)
        {
            groupTimer = 0f;
            StartCoroutine(SpawnGroup(groupSize, progression.inGroupStagger, weighted));
        }
    }

    Dictionary<EnemyData, int> BuildActiveWeights(float elapsed)
    {
        var map = new Dictionary<EnemyData, int>();
        foreach (var w in progression.windows)
        {
            if (w.enemy == null || w.weight <= 0) continue;

            bool started = elapsed >= w.startTime;
            bool notEnded = (w.endTime < 0f) || (elapsed < w.endTime);
            if (started && notEnded)
            {
                if (!map.ContainsKey(w.enemy)) map[w.enemy] = 0;
                map[w.enemy] += w.weight;
            }
        }
        return map;
    }

    IEnumerator SpawnGroup(int groupSize, float inGroupStagger, Dictionary<EnemyData, int> weighted)
    {
        Vector2 playerPos = GameManager.Instance.PlayerTransform.position;
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 center = playerPos + dir * progression.spawnDistance;

        var offsets = GetFormationOffsets(formation, groupSize, progression.formationSpacing, dir);

        for (int i = 0; i < offsets.Count; i++)
        {
            var data = PickWeighted(weighted);
            if (data != null)
            {
                // ===== 추가 시작 =====
                // Ensure the hub sets EnemyData onto the spawned instance (inside hub)
                // Here we just request spawn; the hub should assign eb.data = data internally.
                // ===== 추가 끝 =====
                poolHub.Spawn(data, center + offsets[i], Quaternion.identity);
            }

            if (inGroupStagger > 0f)
                yield return new WaitForSeconds(inGroupStagger);
        }
    }

    EnemyData PickWeighted(Dictionary<EnemyData, int> weighted)
    {
        int total = 0;
        foreach (var kv in weighted) total += kv.Value;
        if (total <= 0) return null;

        int r = Random.Range(0, total);
        foreach (var kv in weighted)
        {
            if (r < kv.Value) return kv.Key;
            r -= kv.Value;
        }
        foreach (var kv in weighted) return kv.Key;
        return null;
    }

    List<Vector2> GetFormationOffsets(FormationType type, int count, float spacing, Vector2 forward)
    {
        var result = new List<Vector2>(count);
        Vector2 right = new Vector2(-forward.y, forward.x).normalized;

        switch (type)
        {
            case FormationType.Line:
                for (int i = 0; i < count; i++)
                {
                    float idx = i - (count - 1) * 0.5f;
                    result.Add(right * idx * spacing);
                }
                break;

            case FormationType.Circle:
                float radius = Mathf.Max(spacing * 0.75f * Mathf.Sqrt(count), spacing);
                for (int i = 0; i < count; i++)
                {
                    float t = i / (float)count;
                    float th = t * Mathf.PI * 2f;
                    result.Add(new Vector2(Mathf.Cos(th), Mathf.Sin(th)) * radius);
                }
                break;

            case FormationType.Arc:
            default:
                float arcAngle = Mathf.PI * 0.8f;
                float radiusArc = Mathf.Max(spacing * 0.6f * Mathf.Sqrt(count), spacing);
                Vector2 face = -forward;
                float baseAngle = Mathf.Atan2(face.y, face.x);
                if (count == 1) { result.Add(face * radiusArc); break; }
                for (int i = 0; i < count; i++)
                {
                    float t = i / (float)(count - 1);
                    float th = baseAngle + Mathf.Lerp(-arcAngle * 0.5f, arcAngle * 0.5f, t);
                    result.Add(new Vector2(Mathf.Cos(th), Mathf.Sin(th)) * radiusArc);
                }
                break;
        }
        return result;
    }
}

public enum FormationType { Line, Circle, Arc }
