using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    Vector2 moveInput;

    void Update()
    {
        // PC: Ű����, �����: ���̽�ƽ���� ���� ����
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        transform.Translate(moveInput * moveSpeed * Time.deltaTime);
    }

    /*
     * 
     public float moveSpeed = 5f;
    public Joystick joystick; // JoystickPack�� Joystick ������Ʈ ����

    void Update()
    {
        Vector2 moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        moveInput = moveInput.normalized;

        transform.Translate(moveInput * moveSpeed * Time.deltaTime);
    }
     */
}
