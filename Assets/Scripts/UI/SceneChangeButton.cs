using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] private Button myButton;

    private void Start()
    {
        // ��ư�� �̺�Ʈ ����
        myButton.onClick.AddListener(OnClickChangeScene);
    }

    private void OnClickChangeScene()
    {
        SceneManager.LoadScene("MainMenu");
        // "MainMenu"�� ��ȯ�� �� �̸�
    }
}
