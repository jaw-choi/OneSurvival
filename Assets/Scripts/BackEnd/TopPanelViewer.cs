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
		// �г����� �������� ������ gamerId�� ǥ���ϰ�, �г����� �����ϸ� �г����� ǥ��
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
            //textNickname.text = loggedIn ? UserInfo.Data.nickname : "�Խ�Ʈ";
            if (loggedIn)
            {
                if(UserInfo.Data.nickname == null)
                {
                    textNickname.text = "No Name";
                }
            }
        }
        if (loginButton) loginButton.SetActive(!loggedIn);

        // ��� UI,Kill UI ��� �ʱ�ȭ
        UpdateGameData();
    }

    public void UpdateGameData()
    {
        if (textKillNumber)
            textKillNumber.text = $"�� óġ �� : {BackendGameData.Instance.UserGameData.score}";
        if (textGold)
        {
            if (UserInfo.IsLoggedIn && BackendGameData.Instance != null)
            {
                // ���� ������ ���
                textGold.text = $"Gold : {BackendGameData.Instance.UserGameData.gold}";

            }
            else
            {
                // ���� PlayerPrefs ���
                if(GoldManager.Instance)
                textGold.text = $"Gold : {GoldManager.Instance.Gold}";

            }
        }
    }

}
