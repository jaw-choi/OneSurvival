using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using UnityEngine.SceneManagement;

public class RegisterAccount : LoginBase
{
	[SerializeField]
	private Image imageID;              // ID �Է� �ʵ� �׵θ� �̹���
	[SerializeField]
	private TMP_InputField inputFieldID;            // ID �Է� �ؽ�Ʈ �ʵ�
	[SerializeField]
	private Image imagePW;              // PW �Է� �ʵ� �׵θ� �̹���
	[SerializeField]
	private TMP_InputField inputFieldPW;            // PW �Է� �ؽ�Ʈ �ʵ�
	[SerializeField]
	private Image imageConfirmPW;           // Confirm PW �Է� �ʵ� �׵θ� �̹���
	[SerializeField]
	private TMP_InputField inputFieldConfirmPW; // Confirm PW �Է� �ؽ�Ʈ �ʵ�
	[SerializeField]
	private Image imageEmail;               // E-mail �Է� �ʵ� �׵θ� �̹���
	[SerializeField]
	private TMP_InputField inputFieldEmail;     // E-mail �Է� �ؽ�Ʈ �ʵ�

	[SerializeField]
	private Button btnRegisterAccount;      // "���� ����" ��ư (��Ȱ��ȭ/Ȱ��ȭ)

	/// <summary>
	/// "���� ����" ��ư�� ������ �� ȣ��
	/// </summary>
	public void OnClickRegisterAccount()
	{
		// �Ķ���ͷ� �Էµ� InputField UI�� �׵θ� �� Message �ʱ�ȭ
		ResetUI(imageID, imagePW, imageConfirmPW, imageEmail);

		// �� �ʵ尡 ����ִ��� Ȯ��
		if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
		if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "��й�ȣ")) return;
		if (IsFieldDataEmpty(imageConfirmPW, inputFieldConfirmPW.text, "��й�ȣ Ȯ��")) return;
		// ��й�ȣ�� ��й�ȣ Ȯ���� ��ġ���� �ʴ� ���
		if (!inputFieldPW.text.Equals(inputFieldConfirmPW.text))
		{
			GuideForIncorrectlyEnteredData(imageConfirmPW, "��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
			return;
		}
		if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "�̸��� �ּ�")) return;


		// �̸��� ���� Ȯ��
		if (!inputFieldEmail.text.Contains("@"))
		{
			GuideForIncorrectlyEnteredData(imageEmail, "�̸��� ������ �ùٸ��� �ʽ��ϴ�.(ex. address@xx.xx)");
			return;
		}

		// "���� ����" ��ư�� ��Ȱ��ȭ
		btnRegisterAccount.interactable = false;
		SetMessage("���� ���� ���Դϴ�.");

		// ���� ���� �õ�
		CustomSignUp();
	}

	/// <summary>
	/// ���� ���� �õ� �� �ݹ鿡�� ��ȯ�� message�� ó���ϴ� �κ�
	/// </summary>
	private void CustomSignUp()
	{
		Backend.BMember.CustomSignUp(inputFieldID.text, inputFieldPW.text, callback =>
		{
			// "���� ����" ��ư �ٽ� Ȱ��ȭ
			btnRegisterAccount.interactable = true;

			// ���� ���� ����
			if (callback.IsSuccess())
			{
				// E-mail ���� ������Ʈ
				Backend.BMember.UpdateCustomEmail(inputFieldEmail.text, callback =>
				{
					if (callback.IsSuccess())
					{
						SetMessage($"���� ���� �Ϸ�. {inputFieldID.text}�� ȯ���մϴ�.");

						// Lobby ������ �̵�
						SceneManager.LoadScene("MainMenu");
					}
				});
			}
			// ���� ���� ����
			else
			{
				string message = string.Empty;

				switch (int.Parse(callback.GetStatusCode()))
				{
					case 409:   // �ߺ��� customId�� �����ϴ� ���
						message = "�̹� �����ϴ� ���̵��Դϴ�.";
						break;
					case 403:   // �������� ������ ��û�� ���
						message = callback.GetMessage();
						break;
					case 401:   // Ŭ���̾�Ʈ ���°� '�α���'�� ���
					case 400:   // ��û �����Ͱ� null�� ���
					default:
						message = callback.GetMessage();
						break;
				}

				if (message.Contains("���̵�"))
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
