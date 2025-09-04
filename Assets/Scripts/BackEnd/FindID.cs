using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class FindID : LoginBase
{
	[SerializeField]
	private Image imageEmail;           // E-mail �Է� �ʵ� �׵θ� �̹���
	[SerializeField]
	private TMP_InputField inputFieldEmail; // E-mail �Է� �ؽ�Ʈ �ʵ�

	[SerializeField]
	private Button btnFindID;           // "���̵� ã��" ��ư (��Ȱ��ȭ/Ȱ��ȭ)

	public void OnClickFindID()
	{
		// �Ķ���ͷ� �Էµ� InputField UI�� �׵θ� �� Message �ʱ�ȭ
		ResetUI(imageEmail);

		// �ʵ尡 ����ִ��� Ȯ��
		if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "�̸��� �ּ�")) return;

		// �̸��� ���� Ȯ��
		if (!inputFieldEmail.text.Contains("@"))
		{
			GuideForIncorrectlyEnteredData(imageEmail, "�̸��� ������ �ùٸ��� �ʽ��ϴ�.(ex. address@xx.xx)");
			return;
		}

		// "���̵� ã��" ��ư ��Ȱ��ȭ
		btnFindID.interactable = false;
		SetMessage("��û�� ���� ���Դϴ�.");

		// ������ ���̵� ã�� �õ�
		FindCustomID();
	}

	/// <summary>
	/// ���̵� ã�⸦ ���� �Էµ� �̸��Ϸ� ��û�� ������ ��,
	/// �ݹ鿡�� ��ȯ�� message�� ó���ϴ� �κ�
	/// </summary>
	private void FindCustomID()
	{
		// ���̵� ã�� ��û (�̸��� ����)
		Backend.BMember.FindCustomID(inputFieldEmail.text, callback =>
		{
			// "���̵� ã��" ��ư �ٽ� Ȱ��ȭ
			btnFindID.interactable = true;

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
					case 404:   // �ش� �̸��Ϸ� ���Ե� ������ ���� ���
						message = "�ش� �̸��Ϸ� ���Ե� ����ڰ� �������� �ʽ��ϴ�.";
						break;
					case 429:   // 24�ð� �̳��� 5ȸ �̻� �̸����� ���� ���̵�/��й�ȣ ã�⸦ �õ��� ���
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
