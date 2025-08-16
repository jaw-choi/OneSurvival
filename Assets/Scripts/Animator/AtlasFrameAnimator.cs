using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.U2D;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class AtlasFrameAnimator : MonoBehaviour
{
    [Header("Atlas / Naming")]
    public SpriteAtlas atlas;          // 아틀라스 할당(Enemy1Atlas 등)

    public string walkPrefix = "walk-";
    public string deadPrefix = "dead-";

    [Header("Playback")]
    public float deadFps = 6f;         // dead FPS
    public float walkFps = 10f;        // Walk FPS
    public bool randomStartOffset = true; // 시작 프레임 랜덤 오프셋
    public bool loopDead = false;
    [Range(0.05f, 3f)]
    public float speedScale = 0.5f;  // ← 기본을 0.6 정도로 시작해 보세요
    public enum State { Dead, Walk }
    public State state = State.Dead;

    private SpriteRenderer sr;
    private Sprite[] deadFrames;
    private Sprite[] walkFrames;
    private float timer;
    private int frame;
    public System.Action<State> onCompleted;
    public bool completedFired = false;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // 아틀라스에서 이름 규칙에 맞는 스프라이트들을 수집
        deadFrames = LoadFrames(deadPrefix);
        walkFrames = LoadFrames(walkPrefix);
    }

    void OnEnable()
    {
        timer = 0f;
        var frames = GetFrames();

        // Dead에는 랜덤 오프셋 주지 않음(보통 처음부터)
        frame = (randomStartOffset && frames.Length > 0 && state != State.Dead)
              ? Random.Range(0, frames.Length)
              : 0;

        if (frames.Length > 0) sr.sprite = frames[frame];
        else Debug.LogWarning($"[AtlasFrameAnimator] '{state}' 프레임이 비어있습니다. obj={name}");
    }

    void Update()
    {
        var frames = GetFrames();
        if (frames == null || frames.Length == 0) return;

        float fps = (state == State.Dead) ? deadFps : walkFps;
        if (fps <= 0f) fps = 1f;

        // Dead 비루프: 이미 마지막 프레임이면 더 이상 진행하지 않음
        if (state == State.Dead && !loopDead && frame >= frames.Length - 1)
        {
            if (sr.sprite != frames[frame]) sr.sprite = frames[frame];
            if (!completedFired)
            {
                completedFired = true;
                onCompleted?.Invoke(State.Dead);
            }
            return;
        }

        timer += Time.deltaTime * speedScale;
        float step = 1f / fps;

        while (timer >= step)
        {
            timer -= step;

            // 다음 프레임 계산 (Dead 비루프는 오버슈트 금지)
            int next = frame + 1;

            if (state == State.Dead && !loopDead && next >= frames.Length)
            {
                // 마지막 프레임에 고정하고 루프 종료(여기서 sprite를 바꾸지 않으면 '4 찍고 3' 현상 방지)
                frame = frames.Length - 1;
                // 최종 프레임을 한 번만 세팅
                if (sr.sprite != frames[frame]) sr.sprite = frames[frame];
                break;
            }

            // Walk(루프) 또는 Dead(루프 허용) 처리
            if (next >= frames.Length)
                next = 0;

            frame = next;
            if (sr.sprite != frames[frame]) sr.sprite = frames[frame];
        }
    }

    public void SetState(State s)
    {
        if (state == s) return;
        state = s;
        timer = 0f;
        frame = 0;
        var frames = GetFrames();
        if (frames.Length > 0) sr.sprite = frames[0];
    }

    private Sprite[] GetFrames()
    {
        return state == State.Dead
            ? (deadFrames ?? System.Array.Empty<Sprite>())
            : (walkFrames ?? System.Array.Empty<Sprite>());
    }

    private Sprite[] LoadFrames(string prefix)
    {
        if (atlas == null) return System.Array.Empty<Sprite>();

        var all = new Sprite[atlas.spriteCount];
        atlas.GetSprites(all);

        var num = new Regex(@"\d+$");
        var list = all.Where(s => s && s.name.StartsWith(prefix))
                      .OrderBy(s =>
                      {
                          var m = num.Match(s.name);
                          return m.Success ? int.Parse(m.Value) : int.MaxValue;
                      })
                      .ToArray();

        // ↓↓↓ 프레임을 못 찾았을 때 원인 파악용 로그
        if (list.Length == 0)
        {
            Debug.LogWarning($"[AtlasFrameAnimator] prefix '{prefix}'로 스프라이트를 못 찾았습니다. atlas={atlas.name}, obj={name}");
            var preview = string.Join(", ", all.Take(8).Select(s => s ? s.name : "null"));
            Debug.Log($"[AtlasFrameAnimator] atlas 내 이름 예시: {preview}");
        }
        // ↑↑↑

        return list;
    }
}
