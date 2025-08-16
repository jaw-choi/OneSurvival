using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.U2D;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class AtlasFrameAnimator : MonoBehaviour
{
    [Header("Atlas / Naming")]
    public SpriteAtlas atlas;          // ��Ʋ�� �Ҵ�(Enemy1Atlas ��)

    public string walkPrefix = "walk-";
    public string deadPrefix = "dead-";

    [Header("Playback")]
    public float deadFps = 6f;         // dead FPS
    public float walkFps = 10f;        // Walk FPS
    public bool randomStartOffset = true; // ���� ������ ���� ������
    public bool loopDead = false;
    [Range(0.05f, 3f)]
    public float speedScale = 0.5f;  // �� �⺻�� 0.6 ������ ������ ������
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

        // ��Ʋ�󽺿��� �̸� ��Ģ�� �´� ��������Ʈ���� ����
        deadFrames = LoadFrames(deadPrefix);
        walkFrames = LoadFrames(walkPrefix);
    }

    void OnEnable()
    {
        timer = 0f;
        var frames = GetFrames();

        // Dead���� ���� ������ ���� ����(���� ó������)
        frame = (randomStartOffset && frames.Length > 0 && state != State.Dead)
              ? Random.Range(0, frames.Length)
              : 0;

        if (frames.Length > 0) sr.sprite = frames[frame];
        else Debug.LogWarning($"[AtlasFrameAnimator] '{state}' �������� ����ֽ��ϴ�. obj={name}");
    }

    void Update()
    {
        var frames = GetFrames();
        if (frames == null || frames.Length == 0) return;

        float fps = (state == State.Dead) ? deadFps : walkFps;
        if (fps <= 0f) fps = 1f;

        // Dead �����: �̹� ������ �������̸� �� �̻� �������� ����
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

            // ���� ������ ��� (Dead ������� ������Ʈ ����)
            int next = frame + 1;

            if (state == State.Dead && !loopDead && next >= frames.Length)
            {
                // ������ �����ӿ� �����ϰ� ���� ����(���⼭ sprite�� �ٲ��� ������ '4 ��� 3' ���� ����)
                frame = frames.Length - 1;
                // ���� �������� �� ���� ����
                if (sr.sprite != frames[frame]) sr.sprite = frames[frame];
                break;
            }

            // Walk(����) �Ǵ� Dead(���� ���) ó��
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

        // ���� �������� �� ã���� �� ���� �ľǿ� �α�
        if (list.Length == 0)
        {
            Debug.LogWarning($"[AtlasFrameAnimator] prefix '{prefix}'�� ��������Ʈ�� �� ã�ҽ��ϴ�. atlas={atlas.name}, obj={name}");
            var preview = string.Join(", ", all.Take(8).Select(s => s ? s.name : "null"));
            Debug.Log($"[AtlasFrameAnimator] atlas �� �̸� ����: {preview}");
        }
        // ����

        return list;
    }
}
