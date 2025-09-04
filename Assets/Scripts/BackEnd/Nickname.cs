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
	private Image imageNickname;              // �г��� �Է� �ʵ� �׵θ� �̹���
	[SerializeField]
	private TMP_InputField inputFieldNickname; // �г��� �Է� �ؽ�Ʈ �ʵ�

	[SerializeField]
	private Button btnUpdateNickname;         // "�г��� ����" ��ư (��Ȱ��/Ȱ�� ��ȯ)

	private void OnEnable()
	{
		// �г��� ���� �˾��� ���� ������ UI ���� �ʱ�ȭ
		ResetUI(imageNickname);
		SetMessage("�г����� �Է��� �ּ���.");
	}

	public void OnClickUpdateNickname()
	{
		// �Է� �ʵ� �׵θ�/�޽��� �ʱ�ȭ
		ResetUI(imageNickname);

		// �� �� ����
		if (IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "�г���")) return;

		// "�г��� ����" ��ư ��Ȱ��ȭ �� �ȳ� �޽��� ǥ��
		btnUpdateNickname.interactable = false;
		SetMessage("�г����� ���� ���Դϴ�...");

		// �г��� ���� �õ�
		UpdateNickname();
	}

	private void UpdateNickname()
	{
		// �г��� ���� ��û
		Backend.BMember.UpdateNickname(inputFieldNickname.text, callback =>
		{
			// ��û ���� �� ��ư �ٽ� Ȱ��ȭ
			btnUpdateNickname.interactable = true;

			// ���� ó��
			if (callback.IsSuccess())
			{
				SetMessage($"{inputFieldNickname.text}(��)�� �г����� ����Ǿ����ϴ�.");

				// �г��� ���� �Ϸ� �̺�Ʈ ȣ�� (UI ���� �� ����)
				onNicknameEvent?.Invoke();
			}
			// ���� ó��
			else
			{
				string message;

				switch (int.Parse(callback.GetStatusCode()))
				{
					case 400: // �� �г���, 20�� �̻�, Ư������ ���� �� ��Ģ ����
						message = "�г����� ����ų� | 20�� �̻��̰ų� | Ư�����ڸ� �����ϰ� �ֽ��ϴ�.";
						break;
					case 409: // �̹� �����ϴ� �г���
						message = "�̹� �����ϴ� �г����Դϴ�.";
						break;
					default:
						message = callback.GetMessage();
						break;
				}

				// �߸��� �Է¿� ���� �ð��� �ȳ�
				GuideForIncorrectlyEnteredData(imageNickname, message);
			}
		});
	}
}
