using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using UnityEngine.SceneManagement;

public class Login : LoginBase
{
    [SerializeField]
    private Image imageID;                    // ID �Է� �ʵ��� ��� �̹���
    [SerializeField]
    private TMP_InputField inputFieldID;      // ID �Է� �ؽ�Ʈ �Է�â
    [SerializeField]
    private Image imagePW;                    // PW �Է� �ʵ��� ��� �̹���
    [SerializeField]
    private TMP_InputField inputFieldPW;      // PW �Է� �ؽ�Ʈ �Է�â

    [SerializeField]
    private Button btnLogin;                  // �α��� ��ư (���¿� ���� Ȱ��/��Ȱ��)

    /// <summary>
    /// "�α���" ��ư Ŭ�� �� ȣ��
    /// </summary>
    public void OnClickLogin()
    {
        // �Է� �ʵ��� ���� �� �ȳ� �޽��� �ʱ�ȭ
        ResetUI(imageID, imagePW);

        // ���̵�� ��й�ȣ�� ��� �ִ��� Ȯ��
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "��й�ȣ")) return;

        // �α��� ��ư�� ��Ȱ��ȭ�Ͽ� �ߺ� Ŭ�� ����
        btnLogin.interactable = false;

        // �α��� ��û ������ ȭ�鿡 �����ֱ� ���� �ڷ�ƾ ����
        StartCoroutine(nameof(LoginProcess));

        // ������ �α��� ��û
        ResponseToLogin(inputFieldID.text, inputFieldPW.text);
    }

    /// <summary>
    /// �α��� ��û �� ���� ���� ó��
    /// </summary>
    private void ResponseToLogin(string ID, string PW)
    {
        // ������ �α��� ��û
        Backend.BMember.CustomLogin(ID, PW, callback =>
        {
            // �α��� ���μ��� �ڷ�ƾ ����
            StopCoroutine(nameof(LoginProcess));

            // �α��� ����
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldID.text}�� ȯ���մϴ�.");

                if (GoldManager.Instance != null)
                {
                    GoldManager.Instance.LoadGold();
                }
                // MainMenu ������ �̵�
                SceneManager.LoadScene("MainMenu");
            }
            // �α��� ����
            else
            {
                // ���� �� ��ư�� �ٽ� Ȱ��ȭ�Ͽ� ��õ� �����ϰ� ��
                btnLogin.interactable = true;

                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 401:   // �������� �ʴ� ���̵� �Ǵ� �߸��� ��й�ȣ
                        message = callback.GetMessage().Contains("customId")
                            ? "�������� �ʴ� ���̵��Դϴ�."
                            : "�߸��� ��й�ȣ�Դϴ�.";
                        break;
                    case 403:   // ���ܵ� ���� �Ǵ� ���ܵ� ����̽�
                        message = callback.GetMessage().Contains("user")
                            ? "���ܵ� �����Դϴ�."
                            : "���ܵ� ����̽��Դϴ�.";
                        break;
                    case 410:   // Ż���� ȸ��
                        message = "Ż���� ȸ���Դϴ�.";
                        break;
                    default:    // �� �� ���� ���� �޽���
                        message = callback.GetMessage();
                        break;
                }

                // �߸��� ��й�ȣ�� ��� ��й�ȣ �Է�ĭ ����
                if (message.Contains("��й�ȣ"))
                {
                    GuideForIncorrectlyEnteredData(imagePW, message);
                }
                // �� �ܿ��� ���̵� �Է�ĭ ����
                else
                {
                    GuideForIncorrectlyEnteredData(imageID, message);
                }
            }
        });
    }

    /// <summary>
    /// �α��� ���� ���� �� ȣ��Ǵ� �ڷ�ƾ
    /// "�α��� ���Դϴ�... n.n��" ���·� �޽����� ��� ����
    /// </summary>
    private IEnumerator LoginProcess()
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;

            SetMessage($"�α��� ���Դϴ�... {time:F1}");

            yield return null;
        }
    }
}

/* -------------------- ���� ��� �α��� ���� --------------------

// ���� ��� �α��� ��û
var bro = Backend.BMember.CustomLogin(ID, PW);

// �α��� ����
if ( bro.IsSuccess() )
{
    Debug.Log($"�α��� ���� : {bro.GetStatusCode()}");
}
// �α��� ����
else
{
    string message = string.Empty;

    switch ( int.Parse(bro.GetStatusCode()) )
    {
        case 401:   // �������� �ʴ� ���̵�, �߸��� ��й�ȣ
            message = bro.GetMessage().Contains("customId") 
                ? "�������� �ʴ� ���̵��Դϴ�." 
                : "�߸��� ��й�ȣ�Դϴ�.";
            break;
        case 403:   // ���ܵ� ���� �Ǵ� ���ܵ� ����̽�
            message = bro.GetMessage().Contains("user") 
                ? "���ܵ� �����Դϴ�." 
                : "���ܵ� ����̽��Դϴ�.";
            break;
        case 410:   // Ż���� ȸ��
            message = "Ż���� ȸ���Դϴ�.";
            break;
        default:
            message = bro.GetMessage();
            break;
    }

    if ( message.Contains("��й�ȣ") )
    {
        GuideForIncorrectlyEnteredData(imagePW, message);
    }
    else
    {
        GuideForIncorrectlyEnteredData(imageID, message);
    }
}
-------------------------------------------------------------- */
