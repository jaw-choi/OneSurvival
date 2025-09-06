using UnityEngine;
using BackEnd;
using LitJson;

public class UserInfo : MonoBehaviour
{
	[System.Serializable]
	public class UserInfoEvent : UnityEngine.Events.UnityEvent { }
	public UserInfoEvent onUserInfoEvent = new UserInfoEvent();

	private static UserInfoData data = new UserInfoData();
	public static UserInfoData Data => data;
	public static bool IsLoggedIn => data.isLoggedIn;

	public void GetUserInfoFromBackend()
	{
		// 현재 로그인된 사용자의 정보를 조회
		// https://developer.thebackend.io/unity3d/guide/bmember/userInfo/
		Backend.BMember.GetUserInfo(callback =>
		{
			// 정보 조회 성공
			if (callback.IsSuccess())
			{
				// JSON 결과 파싱 시도
				try
				{
					JsonData json = callback.GetReturnValuetoJSON()["row"];

					data.gamerId = json["gamerId"].ToString();                 // 게이머 ID
					data.countryCode = json["countryCode"]?.ToString();             // 국가 코드(없으면 null)
					data.nickname = json["nickname"]?.ToString();                // 닉네임(없으면 null)
					data.inDate = json["inDate"].ToString();                   // 가입 일시
					data.emailForFindPassword = json["emailForFindPassword"]?.ToString();    // 비밀번호 찾기용 이메일(없으면 null)
					data.subscriptionType = json["subscriptionType"].ToString();         // 가입 유형(커스텀/연동 등)
					data.federationId = json["federationId"]?.ToString();            // 연동 계정 ID(커스텀 가입 시 null)
				}
				// JSON 파싱 예외 처리
				catch (System.Exception e)
				{
					// 실패 시 기본값으로 초기화
					data.Reset();
					// 예외 로그
					Debug.LogError(e);
				}
				data.isLoggedIn = true;
			}
			// 정보 조회 실패
			else
			{
				// 실패 시 기본값으로 초기화
				// Tip. 일반적으로 네트워크/세션 이슈로 실패할 수 있으므로 다음 프레임/재시도 시점에 다시 조회하는 것도 고려
				data.Reset();
				//Debug.LogError(callback.GetMessage());
				//TODO : mainmenu에서 캐릭터 아이콘 대신 ? 아이콘 보이게하기 ( bool값으로 login했는지 안했는지 getter만들기)
				//로그인하기 버튼활성화 하기
				//이후 로그인했으면 버튼 비활성화하기
			}

			// 사용자 정보 조회 완료 시점에 onUserInfoEvent에 연결된 이벤트 호출
			onUserInfoEvent?.Invoke();
		});
	}
}

public class UserInfoData
{
	
	public string gamerId;              // 게이머 ID
	public string countryCode;          // 국가 코드(없으면 "Unknown")
	public string nickname;             // 닉네임(없으면 "Noname")
	public string inDate;                   // 가입 일시
	public string emailForFindPassword; // 비밀번호 찾기용 이메일
	public string subscriptionType;     // 가입 유형(커스텀/연동 등)
	public string federationId;         // 연동 로그인 ID(커스텀 가입이면 빈 문자열)
	public bool isLoggedIn;

	public void Reset()
	{
		gamerId = "Offline";
		countryCode = "Unknown";
		nickname = "Noname";
		inDate = string.Empty;
		emailForFindPassword = string.Empty;
		subscriptionType = string.Empty;
		federationId = string.Empty;
		isLoggedIn = false;
	}
}
