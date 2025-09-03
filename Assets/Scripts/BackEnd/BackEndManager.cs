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
            Debug.Log($"초기화 성공 : {bro}");
        }
        else
        {
            Debug.LogError($"초기화 실패 : {bro}");

        }
    }
}
