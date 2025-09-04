using UnityEngine;
using TMPro;

public class PopupUpdateProfileViewer : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI textNickname;
	[SerializeField]
	private TextMeshProUGUI textGamerID;

	public void UpdateNickname()
	{
		// �г����� �������� ������ gamerId�� ǥ���ϰ�, �г����� �����ϸ� �г����� ǥ��
		textNickname.text = UserInfo.Data.nickname == null ?
							UserInfo.Data.gamerId : UserInfo.Data.nickname;

		// gamerId ǥ��
		textGamerID.text = UserInfo.Data.gamerId;
	}
}
