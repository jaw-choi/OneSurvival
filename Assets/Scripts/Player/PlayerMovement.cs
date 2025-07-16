using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    Vector2 moveInput;

    void Update()
    {
        // PC: 키보드, 모바일: 조이스틱으로 변경 가능
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        transform.Translate(moveInput * moveSpeed * Time.deltaTime);
    }

    /*
     * 
     public float moveSpeed = 5f;
    public Joystick joystick; // JoystickPack의 Joystick 컴포넌트 연결

    void Update()
    {
        Vector2 moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        moveInput = moveInput.normalized;

        transform.Translate(moveInput * moveSpeed * Time.deltaTime);
    }
     */
}
