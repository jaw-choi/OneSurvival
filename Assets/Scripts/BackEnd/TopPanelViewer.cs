using UnityEngine;
using TMPro;

public class TopPanelViewer : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI textNickname;

	public void UpdateNickname()
	{
		// 닉네임이 존재하지 않으면 gamerId를 표시하고, 닉네임이 존재하면 닉네임을 표시
		textNickname.text = UserInfo.Data.nickname == null ?
							UserInfo.Data.gamerId : UserInfo.Data.nickname;
	}
}
