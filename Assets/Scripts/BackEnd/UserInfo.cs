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

	public void GetUserInfoFromBackend()
	{
		// ���� �α��ε� ������� ������ ��ȸ
		// https://developer.thebackend.io/unity3d/guide/bmember/userInfo/
		Backend.BMember.GetUserInfo(callback =>
		{
			// ���� ��ȸ ����
			if (callback.IsSuccess())
			{
				// JSON ��� �Ľ� �õ�
				try
				{
					JsonData json = callback.GetReturnValuetoJSON()["row"];

					data.gamerId = json["gamerId"].ToString();                 // ���̸� ID
					data.countryCode = json["countryCode"]?.ToString();             // ���� �ڵ�(������ null)
					data.nickname = json["nickname"]?.ToString();                // �г���(������ null)
					data.inDate = json["inDate"].ToString();                   // ���� �Ͻ�
					data.emailForFindPassword = json["emailForFindPassword"]?.ToString();    // ��й�ȣ ã��� �̸���(������ null)
					data.subscriptionType = json["subscriptionType"].ToString();         // ���� ����(Ŀ����/���� ��)
					data.federationId = json["federationId"]?.ToString();            // ���� ���� ID(Ŀ���� ���� �� null)
				}
				// JSON �Ľ� ���� ó��
				catch (System.Exception e)
				{
					// ���� �� �⺻������ �ʱ�ȭ
					data.Reset();
					// ���� �α�
					Debug.LogError(e);
				}
			}
			// ���� ��ȸ ����
			else
			{
				// ���� �� �⺻������ �ʱ�ȭ
				// Tip. �Ϲ������� ��Ʈ��ũ/���� �̽��� ������ �� �����Ƿ� ���� ������/��õ� ������ �ٽ� ��ȸ�ϴ� �͵� ���
				data.Reset();
				Debug.LogError(callback.GetMessage());
			}

			// ����� ���� ��ȸ �Ϸ� ������ onUserInfoEvent�� ����� �̺�Ʈ ȣ��
			onUserInfoEvent?.Invoke();
		});
	}
}

public class UserInfoData
{
	public string gamerId;              // ���̸� ID
	public string countryCode;          // ���� �ڵ�(������ "Unknown")
	public string nickname;             // �г���(������ "Noname")
	public string inDate;                   // ���� �Ͻ�
	public string emailForFindPassword; // ��й�ȣ ã��� �̸���
	public string subscriptionType;     // ���� ����(Ŀ����/���� ��)
	public string federationId;         // ���� �α��� ID(Ŀ���� �����̸� �� ���ڿ�)

	public void Reset()
	{
		gamerId = "Offline";
		countryCode = "Unknown";
		nickname = "Noname";
		inDate = string.Empty;
		emailForFindPassword = string.Empty;
		subscriptionType = string.Empty;
		federationId = string.Empty;
	}
}
