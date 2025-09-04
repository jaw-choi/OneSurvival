using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class Nickname : LoginBase
{
	[System.Serializable]
	public class NicknameEvent : UnityEngine.Events.UnityEvent { }
	public NicknameEvent onNicknameEvent = new NicknameEvent();

	[SerializeField]
	private Image imageNickname;              // 닉네임 입력 필드 테두리 이미지
	[SerializeField]
	private TMP_InputField inputFieldNickname; // 닉네임 입력 텍스트 필드

	[SerializeField]
	private Button btnUpdateNickname;         // "닉네임 변경" 버튼 (비활성/활성 전환)

	private void OnEnable()
	{
		// 닉네임 변경 팝업이 열릴 때마다 UI 상태 초기화
		ResetUI(imageNickname);
		SetMessage("닉네임을 입력해 주세요.");
	}

	public void OnClickUpdateNickname()
	{
		// 입력 필드 테두리/메시지 초기화
		ResetUI(imageNickname);

		// 빈 값 검증
		if (IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "닉네임")) return;

		// "닉네임 변경" 버튼 비활성화 후 안내 메시지 표시
		btnUpdateNickname.interactable = false;
		SetMessage("닉네임을 변경 중입니다...");

		// 닉네임 변경 시도
		UpdateNickname();
	}

	private void UpdateNickname()
	{
		// 닉네임 변경 요청
		Backend.BMember.UpdateNickname(inputFieldNickname.text, callback =>
		{
			// 요청 종료 후 버튼 다시 활성화
			btnUpdateNickname.interactable = true;

			// 성공 처리
			if (callback.IsSuccess())
			{
				SetMessage($"{inputFieldNickname.text}(으)로 닉네임이 변경되었습니다.");

				// 닉네임 변경 완료 이벤트 호출 (UI 갱신 등 연결)
				onNicknameEvent?.Invoke();
			}
			// 실패 처리
			else
			{
				string message;

				switch (int.Parse(callback.GetStatusCode()))
				{
					case 400: // 빈 닉네임, 20자 이상, 특수문자 포함 등 규칙 위반
						message = "닉네임이 비었거나 | 20자 이상이거나 | 특수문자를 포함하고 있습니다.";
						break;
					case 409: // 이미 존재하는 닉네임
						message = "이미 존재하는 닉네임입니다.";
						break;
					default:
						message = callback.GetMessage();
						break;
				}

				// 잘못된 입력에 대한 시각적 안내
				GuideForIncorrectlyEnteredData(imageNickname, message);
			}
		});
	}
}
