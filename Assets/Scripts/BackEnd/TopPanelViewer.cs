using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TopPanelViewer : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private Image avatarImage;
	[SerializeField] private Image noneImage;
	[SerializeField] private GameObject loginButton;
	[SerializeField] private TextMeshProUGUI textKillNumber;
	[SerializeField] private TextMeshProUGUI textGold;
    private const string GOLD_KEY = "PlayerGold";
    private void Awake()
    {
        BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameData);
    }
    public void UpdateNickname()
	{
		// 닉네임이 존재하지 않으면 gamerId를 표시하고, 닉네임이 존재하면 닉네임을 표시
		textNickname.text = UserInfo.Data.nickname == null ?
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;
	}
    void OnEnable()
    {
        Refresh();
        var ui = FindFirstObjectByType<UserInfo>();  
        if (ui != null) ui.onUserInfoEvent.AddListener(Refresh);
    }

    void OnDisable()
    {
        var ui = FindFirstObjectByType<UserInfo>();
        if (ui != null) ui.onUserInfoEvent.RemoveListener(Refresh);

    }

    public void Refresh()
    {
        bool loggedIn = UserInfo.IsLoggedIn;
        if (loggedIn)
        {
            avatarImage.gameObject.SetActive(true);
            noneImage.gameObject.SetActive(false);
        }
        else
        {
            avatarImage.gameObject.SetActive(false);
            noneImage.gameObject.SetActive(true);
        }

        if (textNickname)
        {
            //textNickname.text = loggedIn ? UserInfo.Data.nickname : "게스트";
            if (loggedIn)
            {
                if(UserInfo.Data.nickname == null)
                {
                    textNickname.text = "No Name";
                }
            }
        }
        if (loginButton) loginButton.SetActive(!loggedIn);

        // 골드 UI,Kill UI 즉시 초기화
        UpdateGameData();
    }

    public void UpdateGameData()
    {
        if (textKillNumber)
            textKillNumber.text = $"적 처치 수 : {BackendGameData.Instance.UserGameData.score}";
        if (textGold)
        {
            if (UserInfo.IsLoggedIn && BackendGameData.Instance != null)
            {
                // 서버 데이터 사용
                textGold.text = $"Gold : {BackendGameData.Instance.UserGameData.gold}";

            }
            else
            {
                // 로컬 PlayerPrefs 사용
                if(GoldManager.Instance)
                textGold.text = $"Gold : {GoldManager.Instance.Gold}";

            }
        }
    }

}
