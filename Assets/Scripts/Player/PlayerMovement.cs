using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public Vector2 movement;
    public float moveSpeed = 3f;
    public FloatingJoystick joyStick;

    //[SerializeField] string animSpeedParam = "moveSpeedFactor";
    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        Vector2 moveInput = new Vector2(joyStick.Horizontal, joyStick.Vertical);
        moveInput = moveInput.normalized;

        if (moveInput.x > 0) spriteRenderer.flipX = false;
        else if (moveInput.x < 0) spriteRenderer.flipX = true;

        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        Vector2 nextVec = moveInput * PlayerStats.Instance.TotalMoveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + nextVec);

    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }


}
