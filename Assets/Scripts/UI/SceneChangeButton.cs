using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] private Button myButton;

    private void Start()
    {
        // 버튼에 이벤트 연결
        myButton.onClick.AddListener(OnClickChangeScene);
    }

    private void OnClickChangeScene()
    {
        SceneManager.LoadScene("MainMenu");
        // "MainMenu"는 전환할 씬 이름
    }
}
