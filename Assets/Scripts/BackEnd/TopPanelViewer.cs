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
		// �г����� �������� ������ gamerId�� ǥ���ϰ�, �г����� �����ϸ� �г����� ǥ��
		textNickname.text = UserInfo.Data.nickname == null ?
							"�Խ�Ʈ" : UserInfo.Data.nickname;
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

        if (textNickname) textNickname.text = loggedIn ? UserInfo.Data.nickname : "�Խ�Ʈ";
        if (loginButton) loginButton.SetActive(!loggedIn);
    }
}
