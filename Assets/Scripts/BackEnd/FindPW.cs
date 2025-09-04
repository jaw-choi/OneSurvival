using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class FindPW : LoginBase
{
	[SerializeField]
	private Image imageID;          // ID �Է� �ʵ� �׵θ� �̹���
	[SerializeField]
	private TMP_InputField inputFieldID;        // ID �Է� �ؽ�Ʈ �ʵ�
	[SerializeField]
	private Image imageEmail;           // E-mail �Է� �ʵ� �׵θ� �̹���
	[SerializeField]
	private TMP_InputField inputFieldEmail; // E-mail �Է� �ؽ�Ʈ �ʵ�

	[SerializeField]
	private Button btnFindPW;           // "��й�ȣ ã��" ��ư (��Ȱ��ȭ/Ȱ��ȭ)

	public void OnClickFindPW()
	{
		// �Ķ���ͷ� �Էµ� InputField UI�� �׵θ� �� Message �ʱ�ȭ
		ResetUI(imageID, imageEmail);

		// �ʵ尡 ����ִ��� Ȯ��
		if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
		if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "�̸��� �ּ�")) return;

		// �̸��� ���� Ȯ��
		if (!inputFieldEmail.text.Contains("@"))
		{
			GuideForIncorrectlyEnteredData(imageEmail, "�̸��� ������ �ùٸ��� �ʽ��ϴ�.(ex. address@xx.xx)");
			return;
		}

		// "��й�ȣ ã��" ��ư ��Ȱ��ȭ
		btnFindPW.interactable = false;
		SetMessage("��û�� ���� ���Դϴ�.");

		// ������ ��й�ȣ ã�� �õ�
		FindCustomPW();
	}

	/// <summary>
	/// ��й�ȣ�� �ʱ�ȭ�ϱ� ���� �̸��Ϸ� ��û�� ������ ��,
	/// �ݹ鿡�� ��ȯ�� message�� ó���ϴ� �κ�
	/// </summary>
	private void FindCustomPW()
	{
		// ���ο� ��й�ȣ �߱� ��û (�̸��� ����)
		Backend.BMember.ResetPassword(inputFieldID.text, inputFieldEmail.text, callback =>
		{
			// "��й�ȣ ã��" ��ư �ٽ� Ȱ��ȭ
			btnFindPW.interactable = true;

			// ��û ����
			if (callback.IsSuccess())
			{
				SetMessage($"{inputFieldEmail.text} �ּҷ� ������ �����Ͽ����ϴ�.");
			}
			// ��û ����
			else
			{
				string message = string.Empty;

				switch (int.Parse(callback.GetStatusCode()))
				{
					case 404:   // �ش� �̸���/���̵�� ���Ե� ������ ���� ���
						message = "�ش� �̸��� �Ǵ� ���̵�� ���Ե� ����ڰ� �������� �ʽ��ϴ�.";
						break;
					case 429:   // 24�ð� �̳��� 5ȸ �̻� ���̵�/��й�ȣ ã�⸦ �õ��� ���
						message = "24�ð� �̳��� 5ȸ �̻� ���̵�/��й�ȣ ã�⸦ �õ��Ͽ����ϴ�.";
						break;
					default:
						// statusCode : 400 => Ŭ���̾�Ʈ ��û�� Ư�����ڰ� ���Ե� ��� (�ȳ� �޽��� ������, �������� ���� ��ȯ)
						message = callback.GetMessage();
						break;
				}

				if (message.Contains("�̸���"))
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
