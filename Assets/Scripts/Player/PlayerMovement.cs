using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public Vector2 movement;
    public float moveSpeed = 3f;
    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get horizontal input (-1 for left, 1 for right)
        float move = Input.GetAxisRaw("Horizontal");

        if (move > 0) spriteRenderer.flipX = false;
        else if (move < 0) spriteRenderer.flipX = true;
        // If not moving, do nothing (keep current direction)

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;
        // 애니메이터에 이동 여부 전달
        bool isMoving = movement.sqrMagnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * PlayerStats.Instance.TotalMoveSpeed * Time.fixedDeltaTime);

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
