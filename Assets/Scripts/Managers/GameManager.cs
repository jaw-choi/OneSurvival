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
    //private bool readyToClick = false;
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
        IsGameOver = true;
    }

    public void AddGold(int value)
    {
        Gold += value;
        UIManager.Instance.UpdateGoldUI(Gold);
    }
}