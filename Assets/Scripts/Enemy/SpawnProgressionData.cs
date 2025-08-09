// Assets/Scripts/Data/SpawnProgressionData.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SpawnProgressionData", fileName = "Scriptable Objects/SpawnProgressionData")]
public class SpawnProgressionData : ScriptableObject
{
    [Header("Difficulty time (seconds)")]
    public float difficultyDuration = 200f;

    [Header("Group curves over time (x=time, y=value)")]
    public AnimationCurve groupIntervalSec = AnimationCurve.Linear(0, 2.5f, 300, 0.6f);
    public AnimationCurve groupSize = AnimationCurve.Linear(0, 4f, 300, 20f);

    [Header("In-group spacing/timing")]
    public float inGroupStagger = 0.05f;   // delay between enemies inside a group

    [Header("Placement")]
    public float spawnDistance = 6f;
    public float formationSpacing = 0.6f;

    [System.Serializable]
    public class Window
    {
        [Tooltip("등장 시작 시각(초)")]
        public float startTime = 0f;
        [Tooltip("등장 종료 시각(초). 음수면 무기한")]
        public float endTime = -1f;
        public EnemyData enemy;
        [Min(0)] public int weight = 1;
    }

    [Header("시간창별 등장 설정(Unlock/Lock)")]
    public List<Window> windows = new List<Window>();
}
