using UnityEngine;
using TMPro;

public class TopPanelViewer : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI textNickname;

	public void UpdateNickname()
	{
		// �г����� �������� ������ gamerId�� ǥ���ϰ�, �г����� �����ϸ� �г����� ǥ��
		textNickname.text = UserInfo.Data.nickname == null ?
							UserInfo.Data.gamerId : UserInfo.Data.nickname;
	}
}
