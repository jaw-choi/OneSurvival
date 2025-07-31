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
    //private bool readyToClick = false;
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
        IsGameOver = true;
    }

    public void AddGold(int value)
    {
        Gold += value;
        UIManager.Instance.UpdateGoldUI(Gold);
    }
}