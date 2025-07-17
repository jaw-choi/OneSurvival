using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    Vector2 moveInput;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Get horizontal input (-1 for left, 1 for right)
        float move = Input.GetAxisRaw("Horizontal");

        // If moving right
        if (move > 0)
        {
            spriteRenderer.flipX = false; // Face right
        }
        // If moving left
        else if (move < 0)
        {
            spriteRenderer.flipX = true; // Face left
        }
        // If not moving, do nothing (keep current direction)

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
