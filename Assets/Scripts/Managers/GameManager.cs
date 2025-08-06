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
    public Image fadeImage; // Panel의 Image 컴포넌트
    public TextMeshProUGUI gameOverText;
    public WeaponData defaultWeaponData;
    private float gameStartTime;
    public float ElapsedTime => Time.time - gameStartTime;
    public int Gold { get; private set; } = 0;
    void Awake()
    {
        // 싱글톤 인스턴스 할당
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지
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
        // 무기 1회만 생성
        Weapon weapon = WeaponManager.Instance.AddWeapon(defaultWeaponData);

        // 자동 공격 컴포넌트 초기화
        var autoAttack = FindAnyObjectByType<PlayerAutoAttack>();
        autoAttack.Initialize(weapon);
    }
    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;

        // 1. 결과 데이터 저장
        GameResultData result = GameResultData.Instance;
        result.playTime = Time.timeSinceLevelLoad; // 또는 gameStartTime 따로 기록해도 됨
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

        // 2. 게임 오버 UI 보여주기 (선택)
        //StartCoroutine(ShowGameOverAndLoadResult());
    }

    public void AddGold(int value)
    {
        Gold += value;
        UIManager.Instance.UpdateGoldUI(Gold);
    }
}