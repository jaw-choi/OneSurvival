using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; } = false; // 게임 종료 여부
    public Transform PlayerTransform { get; private set; } // 플레이어 위치 참조
    public CanvasGroup gameOverPanel; // 게임 오버 패널
    public PlayerMovement player; // 플레이어 스크립트
    
    public Image fadeImage; // 페이드용 이미지
    public TextMeshProUGUI gameOverText; // 게임 오버 텍스트
    public WeaponData defaultWeaponData; // 기본 무기 데이터
    public Button quitButton; // 종료 버튼

    private float gameStartTime; // 게임 시작 시각
    public float ElapsedTime => Time.time - gameStartTime; // 경과 시간

    // 싱글톤 인스턴스 초기화
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

    // 게임 시작 시 초기화 실행
    void Start()
    {
        Init();
    }

    // 게임 실행 초기화: 플레이어, 무기, BGM 세팅
    public void Init()
    {
        gameStartTime = Time.time;
        gameOverPanel.gameObject.SetActive(false);

        PlayerTransform = FindAnyObjectByType<PlayerMovement>().transform;
        player = FindAnyObjectByType<PlayerMovement>();

        GameStateManager.Instance.ChangeState(new InGameState());

        // 기본 무기 장착
        Weapon weapon = WeaponManager.Instance.AddWeapon(defaultWeaponData);

        // 자동 공격 초기화
        var autoAttack = FindAnyObjectByType<PlayerAutoAttack>();
        autoAttack.Initialize(weapon);

        IsGameOver = false;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.PlayBGM(true);
    }

    // 게임 진행 상태를 리셋 (재시작 시 호출)
    public void ResetRunState()
    {
        IsGameOver = false;
        gameStartTime = Time.time;
        StopAllCoroutines();
        CancelInvoke();
    }

    // 게임 오버 처리 및 결과 데이터 기록
    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        AudioManager.instance.PlayBGM(false);

        //BackendGameData.Instance.GameDataUpdate();
        // 결과 데이터 저장
        GameResultData result = GameResultData.Instance;
        result.playTime = Time.timeSinceLevelLoad;
        result.totalGold = GoldManager.Instance.Gold;
        result.playerLevel = PlayerExpManager.Instance.currentLevel;
        result.enemyKillCount += EnemyKillCounter.Instance.TotalKills;
        result.characterName = PlayerStats.Instance.characterName;
        result.mapName = SceneManager.GetActiveScene().name;

        // 무기별 결과 기록
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
        //if (UserInfo.IsLoggedIn && BackendGameData.Instance != null)
            BackendGameData.Instance.GameDataUpdate(AfterGameOver);
    }
    public void AfterGameOver()
    {
        //
    }

}
