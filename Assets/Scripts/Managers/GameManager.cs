using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameOver { get; private set; } = false;
    public Transform PlayerTransform { get; private set; }
    public CanvasGroup gameOverPanel;
    public Image fadeImage; // Panel�� Image ������Ʈ
    public TextMeshProUGUI gameOverText;
    public WeaponData defaultWeaponData;
    private float gameStartTime;
    public float ElapsedTime => Time.time - gameStartTime;
    public int Gold { get; private set; } = 0;
    void Awake()
    {
        // �̱��� �ν��Ͻ� �Ҵ�
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� ����
        }
        else
        {
            Destroy(gameObject);
        }
        IsGameOver = false;
    }

    void Start()
    {
        Init();
    }
    public void Init()
    {
        gameStartTime = Time.time;
        gameOverPanel.gameObject.SetActive(false);
        PlayerTransform = FindAnyObjectByType<PlayerMovement>().transform;
        GameStateManager.Instance.ChangeState(new InGameState());
        // ���� 1ȸ�� ����
        Weapon weapon = WeaponManager.Instance.AddWeapon(defaultWeaponData);

        // �ڵ� ���� ������Ʈ �ʱ�ȭ
        var autoAttack = FindAnyObjectByType<PlayerAutoAttack>();
        autoAttack.Initialize(weapon);
    }
    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;

        // 1. ��� ������ ����
        GameResultData result = GameResultData.Instance;
        result.playTime = Time.timeSinceLevelLoad; // �Ǵ� gameStartTime ���� ����ص� ��
        result.totalGold = Gold;
        result.playerLevel = PlayerExpManager.Instance.currentLevel;
        result.enemyKillCount = EnemyKillCounter.Instance.TotalKills;
        result.characterName = PlayerStats.Instance.characterName;
        result.mapName = SceneManager.GetActiveScene().name;

        foreach (var weapon in WeaponManager.Instance.GetAllWeapons())
        {
            var w = new WeaponResult();
            w.weaponName = weapon.weaponData.weaponName;
            w.weaponLevel = weapon.currentLevel;
            w.totalDamage = weapon.TotalDealtDamage;
            w.heldTime = Time.time - weapon.TimeAcquired;
            result.weaponResults.Add(w);
        }

        // 2. ���� ���� UI �����ֱ� (����)
        //StartCoroutine(ShowGameOverAndLoadResult());
    }

    public void AddGold(int value)
    {
        Gold += value;
        UIManager.Instance.UpdateGoldUI(Gold);
    }
}