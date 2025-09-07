using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; } = false; // ���� ���� ����
    public Transform PlayerTransform { get; private set; } // �÷��̾� ��ġ ����
    public CanvasGroup gameOverPanel; // ���� ���� �г�
    public PlayerMovement player; // �÷��̾� ��ũ��Ʈ
    public Image fadeImage; // ���̵�� �̹���
    public TextMeshProUGUI gameOverText; // ���� ���� �ؽ�Ʈ
    public WeaponData defaultWeaponData; // �⺻ ���� ������
    public Button quitButton; // ���� ��ư

    private float gameStartTime; // ���� ���� �ð�
    public float ElapsedTime => Time.time - gameStartTime; // ��� �ð�
    public int Gold { get; private set; } = 0; // ȹ���� ���

    // �̱��� �ν��Ͻ� �ʱ�ȭ
    void Awake()
    {
        IsGameOver = false;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���� ���� �� �ʱ�ȭ ����
    void Start()
    {
        Init();
    }

    // ���� ���� �ʱ�ȭ: �÷��̾�, ����, BGM ����
    public void Init()
    {
        gameStartTime = Time.time;
        gameOverPanel.gameObject.SetActive(false);

        PlayerTransform = FindAnyObjectByType<PlayerMovement>().transform;
        player = FindAnyObjectByType<PlayerMovement>();

        GameStateManager.Instance.ChangeState(new InGameState());

        // �⺻ ���� ����
        Weapon weapon = WeaponManager.Instance.AddWeapon(defaultWeaponData);

        // �ڵ� ���� �ʱ�ȭ
        var autoAttack = FindAnyObjectByType<PlayerAutoAttack>();
        autoAttack.Initialize(weapon);

        IsGameOver = false;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.PlayBGM(true);
    }

    // ���� ���� ���¸� ���� (����� �� ȣ��)
    public void ResetRunState()
    {
        IsGameOver = false;
        Gold = 0;
        gameStartTime = Time.time;
        StopAllCoroutines();
        CancelInvoke();
    }

    // ���� ���� ó�� �� ��� ������ ���
    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        AudioManager.instance.PlayBGM(false);

        // ��� ������ ����
        GameResultData result = GameResultData.Instance;
        result.playTime = Time.timeSinceLevelLoad;
        result.totalGold = Gold;
        result.playerLevel = PlayerExpManager.Instance.currentLevel;
        result.enemyKillCount = EnemyKillCounter.Instance.TotalKills;
        result.characterName = PlayerStats.Instance.characterName;
        result.mapName = SceneManager.GetActiveScene().name;

        // ���⺰ ��� ���
        foreach (var weapon in WeaponManager.Instance.GetAllWeapons())
        {
            var w = new WeaponResult
            {
                weaponName = weapon.weaponData.weaponName,
                weaponLevel = weapon.currentLevel,
                totalDamage = weapon.TotalDealtDamage,
                heldTime = Time.time - weapon.TimeAcquired
            };
            result.weaponResults.Add(w);
        }
    }

    // ��� �߰� �� UI ����
    public void AddGold(int value)
    {
        Gold += value;
        UIManager.Instance.UpdateGoldUI(Gold);
    }
}
