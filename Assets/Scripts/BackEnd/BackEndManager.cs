using UnityEngine;
using BackEnd;

public class BackEndManager : MonoBehaviour
{
    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        BackEndSetup();
    }
    private void Update()
    {
    }
    //void Update() => SendQueue.Poll();
    private void BackEndSetup()
    {
        var bro = Backend.Initialize();

        if(bro.IsSuccess())
        {
            Debug.Log($"�ʱ�ȭ ���� : {bro}");
        }
        else
        {
            Debug.LogError($"�ʱ�ȭ ���� : {bro}");

        }
    }
}
