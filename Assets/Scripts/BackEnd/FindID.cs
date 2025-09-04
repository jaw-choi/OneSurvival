using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class FindID : LoginBase
{
	[SerializeField]
	private Image imageEmail;           // E-mail 입력 필드 테두리 이미지
	[SerializeField]
	private TMP_InputField inputFieldEmail; // E-mail 입력 텍스트 필드

	[SerializeField]
	private Button btnFindID;           // "아이디 찾기" 버튼 (비활성화/활성화)

	public void OnClickFindID()
	{
		// 파라미터로 입력된 InputField UI의 테두리 및 Message 초기화
		ResetUI(imageEmail);

		// 필드가 비어있는지 확인
		if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "이메일 주소")) return;

		// 이메일 형식 확인
		if (!inputFieldEmail.text.Contains("@"))
		{
			GuideForIncorrectlyEnteredData(imageEmail, "이메일 형식이 올바르지 않습니다.(ex. address@xx.xx)");
			return;
		}

		// "아이디 찾기" 버튼 비활성화
		btnFindID.interactable = false;
		SetMessage("요청을 전송 중입니다.");

		// 계정의 아이디 찾기 시도
		FindCustomID();
	}

	/// <summary>
	/// 아이디 찾기를 위해 입력된 이메일로 요청을 전송한 뒤,
	/// 콜백에서 반환된 message를 처리하는 부분
	/// </summary>
	private void FindCustomID()
	{
		// 아이디 찾기 요청 (이메일 전송)
		Backend.BMember.FindCustomID(inputFieldEmail.text, callback =>
		{
			// "아이디 찾기" 버튼 다시 활성화
			btnFindID.interactable = true;

			// 요청 성공
			if (callback.IsSuccess())
			{
				SetMessage($"{inputFieldEmail.text} 주소로 메일을 전송하였습니다.");
			}
			// 요청 실패
			else
			{
				string message = string.Empty;

				switch (int.Parse(callback.GetStatusCode()))
				{
					case 404:   // 해당 이메일로 가입된 유저가 없는 경우
						message = "해당 이메일로 가입된 사용자가 존재하지 않습니다.";
						break;
					case 429:   // 24시간 이내에 5회 이상 이메일을 통한 아이디/비밀번호 찾기를 시도한 경우
						message = "24시간 이내에 5회 이상 아이디/비밀번호 찾기를 시도하였습니다.";
						break;
					default:
						// statusCode : 400 => 클라이언트 요청에 특수문자가 포함된 경우 (안내 메시지 미전송, 서버에서 에러 반환)
						message = callback.GetMessage();
						break;
				}

				if (message.Contains("이메일"))
				{
					GuideForIncorrectlyEnteredData(imageEmail, message);
				}
				else
				{
					SetMessage(message);
				}
			}
		});
	}
}
