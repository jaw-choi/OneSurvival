using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using UnityEngine.SceneManagement;

public class Login : LoginBase
{
    [SerializeField]
    private Image imageID;                    // ID 입력 필드의 배경 이미지
    [SerializeField]
    private TMP_InputField inputFieldID;      // ID 입력 텍스트 입력창
    [SerializeField]
    private Image imagePW;                    // PW 입력 필드의 배경 이미지
    [SerializeField]
    private TMP_InputField inputFieldPW;      // PW 입력 텍스트 입력창

    [SerializeField]
    private Button btnLogin;                  // 로그인 버튼 (상태에 따라 활성/비활성)

    /// <summary>
    /// "로그인" 버튼 클릭 시 호출
    /// </summary>
    public void OnClickLogin()
    {
        // 입력 필드의 상태 및 안내 메시지 초기화
        ResetUI(imageID, imagePW);

        // 아이디와 비밀번호가 비어 있는지 확인
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "비밀번호")) return;

        // 로그인 버튼을 비활성화하여 중복 클릭 방지
        btnLogin.interactable = false;

        // 로그인 요청 중임을 화면에 보여주기 위해 코루틴 실행
        StartCoroutine(nameof(LoginProcess));

        // 서버에 로그인 요청
        ResponseToLogin(inputFieldID.text, inputFieldPW.text);
    }

    /// <summary>
    /// 로그인 요청 후 서버 응답 처리
    /// </summary>
    private void ResponseToLogin(string ID, string PW)
    {
        // 서버에 로그인 요청
        Backend.BMember.CustomLogin(ID, PW, callback =>
        {
            // 로그인 프로세스 코루틴 중지
            StopCoroutine(nameof(LoginProcess));

            // 로그인 성공
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldID.text}님 환영합니다.");

                if (GoldManager.Instance != null)
                {
                    GoldManager.Instance.LoadGold();
                }
                // MainMenu 씬으로 이동
                SceneManager.LoadScene("MainMenu");
            }
            // 로그인 실패
            else
            {
                // 실패 시 버튼을 다시 활성화하여 재시도 가능하게 함
                btnLogin.interactable = true;

                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 401:   // 존재하지 않는 아이디 또는 잘못된 비밀번호
                        message = callback.GetMessage().Contains("customId")
                            ? "존재하지 않는 아이디입니다."
                            : "잘못된 비밀번호입니다.";
                        break;
                    case 403:   // 차단된 계정 또는 차단된 디바이스
                        message = callback.GetMessage().Contains("user")
                            ? "차단된 계정입니다."
                            : "차단된 디바이스입니다.";
                        break;
                    case 410:   // 탈퇴한 회원
                        message = "탈퇴한 회원입니다.";
                        break;
                    default:    // 그 외 서버 응답 메시지
                        message = callback.GetMessage();
                        break;
                }

                // 잘못된 비밀번호일 경우 비밀번호 입력칸 강조
                if (message.Contains("비밀번호"))
                {
                    GuideForIncorrectlyEnteredData(imagePW, message);
                }
                // 그 외에는 아이디 입력칸 강조
                else
                {
                    GuideForIncorrectlyEnteredData(imageID, message);
                }
            }
        });
    }

    /// <summary>
    /// 로그인 진행 중일 때 호출되는 코루틴
    /// "로그인 중입니다... n.n초" 형태로 메시지를 계속 갱신
    /// </summary>
    private IEnumerator LoginProcess()
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;

            SetMessage($"로그인 중입니다... {time:F1}");

            yield return null;
        }
    }
}

/* -------------------- 동기 방식 로그인 예시 --------------------

// 동기 방식 로그인 요청
var bro = Backend.BMember.CustomLogin(ID, PW);

// 로그인 성공
if ( bro.IsSuccess() )
{
    Debug.Log($"로그인 성공 : {bro.GetStatusCode()}");
}
// 로그인 실패
else
{
    string message = string.Empty;

    switch ( int.Parse(bro.GetStatusCode()) )
    {
        case 401:   // 존재하지 않는 아이디, 잘못된 비밀번호
            message = bro.GetMessage().Contains("customId") 
                ? "존재하지 않는 아이디입니다." 
                : "잘못된 비밀번호입니다.";
            break;
        case 403:   // 차단된 계정 또는 차단된 디바이스
            message = bro.GetMessage().Contains("user") 
                ? "차단된 계정입니다." 
                : "차단된 디바이스입니다.";
            break;
        case 410:   // 탈퇴한 회원
            message = "탈퇴한 회원입니다.";
            break;
        default:
            message = bro.GetMessage();
            break;
    }

    if ( message.Contains("비밀번호") )
    {
        GuideForIncorrectlyEnteredData(imagePW, message);
    }
    else
    {
        GuideForIncorrectlyEnteredData(imageID, message);
    }
}
-------------------------------------------------------------- */
