using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon;
    public FloatingJoystick joyStick;
    public Vector2 movement;
    public float moveSpeed = 3f;

    public Transform visualRoot;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Weapon weapon;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        weapon = GetComponent<Weapon>();

    }

    void OnEnable()
    {
        ApplySelectedAnimator();
    }

    void FixedUpdate()
    {
        Vector2 moveInput = (joyStick != null) ? new Vector2(joyStick.Horizontal, joyStick.Vertical) : movement;
        moveInput = moveInput.normalized;

        if (moveInput.x > 0) spriteRenderer.flipX = false;
        else if (moveInput.x < 0) spriteRenderer.flipX = true;

        bool moving = moveInput.sqrMagnitude > 0.01f;
        if (HasParam(animator, IsMoving, AnimatorControllerParameterType.Bool))
            animator.SetBool(IsMoving, moving);

        float speed = (PlayerStats.Instance != null) ? PlayerStats.Instance.TotalMoveSpeed : moveSpeed;
        Vector2 nextVec = moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    void ApplySelectedAnimator()
    {
        if (animCon == null || animCon.Length == 0 || animator == null) return;

        int id = PlayerStats.Instance?.playerID??0;
        PlayerStats.Instance.RecalculateAll();
        animator.runtimeAnimatorController = animCon[id];


        if (id == 1 || id == 3)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
        else
            transform.localScale = Vector3.one;

    }

    static bool HasParam(Animator anim, int hash, AnimatorControllerParameterType type)
    {
        if (anim == null) return false;
        foreach (var p in anim.parameters)
            if (p.type == type && p.nameHash == hash) return true;
        return false;
    }
}
