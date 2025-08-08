using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Refs")]
    public EnemyPooler enemyPooler;

    [Header("Group Spawn")]
    public float initialGroupInterval = 2.5f;   // time between groups
    public float minGroupInterval = 0.6f;
    public int initialGroupSize = 4;
    public int maxGroupSize = 20;
    public float inGroupStagger = 0.05f;        // delay between enemies inside a group

    [Header("Difficulty")]
    public float difficultyRampTime = 300f;     // seconds to reach max difficulty

    [Header("Placement")]
    public float spawnDistance = 6f;            // distance from player (world units)
    public float formationSpacing = 0.6f;       // spacing between units in a formation

    [Header("Formation")]
    public FormationType formation = FormationType.Arc;

    private float timer;

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsGameOver)
            return;

        float t = Mathf.Clamp01(GameManager.Instance.ElapsedTime / difficultyRampTime);

        // scale group interval and size by difficulty
        float groupInterval = Mathf.Lerp(initialGroupInterval, minGroupInterval, t);
        int groupSize = Mathf.RoundToInt(Mathf.Lerp(initialGroupSize, maxGroupSize, t));

        timer += Time.deltaTime;
        if (timer >= groupInterval)
        {
            timer = 0f;
            StartCoroutine(SpawnGroup(groupSize));
        }
    }

    private IEnumerator SpawnGroup(int groupSize)
    {
        // choose a random direction around the player (0..2¥ð)
        Vector2 playerPos = GameManager.Instance.PlayerTransform.position;
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector2 spawnDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        // center of the formation (off-screen)
        Vector2 center = playerPos + spawnDir * spawnDistance;

        // generate offsets for the chosen formation
        List<Vector2> offsets = GetFormationOffsets(formation, groupSize, formationSpacing, spawnDir);

        for (int i = 0; i < offsets.Count; i++)
        {
            GameObject enemy = enemyPooler.GetEnemy();
            if (enemy != null)
            {
                enemy.transform.position = center + offsets[i];
                enemy.transform.rotation = Quaternion.identity;

                // TODO: call enemy reset/init if needed (HP, speed, AI state)
                // enemy.GetComponent<EnemyBase>()?.ResetState();
            }

            // small stagger to avoid perfect overlap / perf spikes
            if (inGroupStagger > 0f)
                yield return new WaitForSeconds(inGroupStagger);
        }
    }

    private List<Vector2> GetFormationOffsets(FormationType type, int count, float spacing, Vector2 forward)
    {
        var result = new List<Vector2>(count);

        // compute right vector (perpendicular to forward)
        Vector2 right = new Vector2(-forward.y, forward.x).normalized;

        switch (type)
        {
            case FormationType.Line:
                // center-aligned line perpendicular to forward
                // e.g., ... -2 -1 0 1 2 ...
                for (int i = 0; i < count; i++)
                {
                    float idx = i - (count - 1) * 0.5f;
                    result.Add(right * idx * spacing);
                }
                break;

            case FormationType.Circle:
                // place around a circle ring
                float radius = Mathf.Max(spacing * 0.75f * Mathf.Sqrt(count), spacing);
                for (int i = 0; i < count; i++)
                {
                    float theta = (Mathf.PI * 2f) * (i / (float)count);
                    Vector2 dir = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
                    result.Add(dir * radius);
                }
                break;

            case FormationType.Arc:
            default:
                // half-ring arc facing the player (arc opens toward the player)
                // distribute over [-arcAngle..+arcAngle]
                float arcAngle = Mathf.PI * 0.8f; // 144 degrees
                float radiusArc = Mathf.Max(spacing * 0.6f * Mathf.Sqrt(count), spacing);
                // forward is away from player; to face the player, flip
                Vector2 face = -forward;
                float baseAngle = Mathf.Atan2(face.y, face.x);

                if (count == 1)
                {
                    result.Add(face * radiusArc);
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        float t = (count == 1) ? 0f : i / (float)(count - 1);
                        float theta = baseAngle + Mathf.Lerp(-arcAngle * 0.5f, arcAngle * 0.5f, t);
                        Vector2 dir = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
                        result.Add(dir * radiusArc);
                    }
                }
                break;
        }

        return result;
    }
}

public enum FormationType
{
    Line,
    Circle,
    Arc
}
