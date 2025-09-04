using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using UnityEngine.SceneManagement;

public class RegisterAccount : LoginBase
{
	[SerializeField]
	private Image imageID;              // ID 입력 필드 테두리 이미지
	[SerializeField]
	private TMP_InputField inputFieldID;            // ID 입력 텍스트 필드
	[SerializeField]
	private Image imagePW;              // PW 입력 필드 테두리 이미지
	[SerializeField]
	private TMP_InputField inputFieldPW;            // PW 입력 텍스트 필드
	[SerializeField]
	private Image imageConfirmPW;           // Confirm PW 입력 필드 테두리 이미지
	[SerializeField]
	private TMP_InputField inputFieldConfirmPW; // Confirm PW 입력 텍스트 필드
	[SerializeField]
	private Image imageEmail;               // E-mail 입력 필드 테두리 이미지
	[SerializeField]
	private TMP_InputField inputFieldEmail;     // E-mail 입력 텍스트 필드

	[SerializeField]
	private Button btnRegisterAccount;      // "계정 생성" 버튼 (비활성화/활성화)

	/// <summary>
	/// "계정 생성" 버튼이 눌렸을 때 호출
	/// </summary>
	public void OnClickRegisterAccount()
	{
		// 파라미터로 입력된 InputField UI의 테두리 및 Message 초기화
		ResetUI(imageID, imagePW, imageConfirmPW, imageEmail);

		// 각 필드가 비어있는지 확인
		if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
		if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "비밀번호")) return;
		if (IsFieldDataEmpty(imageConfirmPW, inputFieldConfirmPW.text, "비밀번호 확인")) return;
		// 비밀번호와 비밀번호 확인이 일치하지 않는 경우
		if (!inputFieldPW.text.Equals(inputFieldConfirmPW.text))
		{
			GuideForIncorrectlyEnteredData(imageConfirmPW, "비밀번호가 일치하지 않습니다.");
			return;
		}
		if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "이메일 주소")) return;


		// 이메일 형식 확인
		if (!inputFieldEmail.text.Contains("@"))
		{
			GuideForIncorrectlyEnteredData(imageEmail, "이메일 형식이 올바르지 않습니다.(ex. address@xx.xx)");
			return;
		}

		// "계정 생성" 버튼을 비활성화
		btnRegisterAccount.interactable = false;
		SetMessage("계정 생성 중입니다.");

		// 계정 생성 시도
		CustomSignUp();
	}

	/// <summary>
	/// 계정 생성 시도 후 콜백에서 반환된 message를 처리하는 부분
	/// </summary>
	private void CustomSignUp()
	{
		Backend.BMember.CustomSignUp(inputFieldID.text, inputFieldPW.text, callback =>
		{
			// "계정 생성" 버튼 다시 활성화
			btnRegisterAccount.interactable = true;

			// 계정 생성 성공
			if (callback.IsSuccess())
			{
				// E-mail 정보 업데이트
				Backend.BMember.UpdateCustomEmail(inputFieldEmail.text, callback =>
				{
					if (callback.IsSuccess())
					{
						SetMessage($"계정 생성 완료. {inputFieldID.text}님 환영합니다.");

						// Lobby 씬으로 이동
						SceneManager.LoadScene("MainMenu");
					}
				});
			}
			// 계정 생성 실패
			else
			{
				string message = string.Empty;

				switch (int.Parse(callback.GetStatusCode()))
				{
					case 409:   // 중복된 customId가 존재하는 경우
						message = "이미 존재하는 아이디입니다.";
						break;
					case 403:   // 서버에서 금지된 요청일 경우
						message = callback.GetMessage();
						break;
					case 401:   // 클라이언트 상태가 '로그인'일 경우
					case 400:   // 요청 데이터가 null일 경우
					default:
						message = callback.GetMessage();
						break;
				}

				if (message.Contains("아이디"))
				{
					GuideForIncorrectlyEnteredData(imageID, message);
				}
				else
				{
					SetMessage(message);
				}
			}
		});
	}
}
