using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TopPanelViewer : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textNickname;
	[SerializeField] private Image avatarImage;
	[SerializeField] private Image noneImage;
	[SerializeField] private GameObject loginButton;

	public void UpdateNickname()
	{
		// 닉네임이 존재하지 않으면 gamerId를 표시하고, 닉네임이 존재하면 닉네임을 표시
		textNickname.text = UserInfo.Data.nickname == null ?
							"게스트" : UserInfo.Data.nickname;
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

        if (textNickname) textNickname.text = loggedIn ? UserInfo.Data.nickname : "게스트";
        if (loginButton) loginButton.SetActive(!loggedIn);
    }
}
