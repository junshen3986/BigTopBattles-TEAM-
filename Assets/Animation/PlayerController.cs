using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Key Bindings")]
    public KeyCode jumpKey = KeyCode.W;
    public KeyCode crouchKey = KeyCode.S;
    public KeyCode blockKey = KeyCode.L;
    public KeyCode lightPunchKey = KeyCode.J;
    public KeyCode heavyPunchKey = KeyCode.K;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;

    // cached components & state
    Rigidbody2D rb;
    Animator anim;
    bool isGrounded;
    float horizontalInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        // You already have Entry → clown_idle, so you can omit anim.Play().
    }

    void Update()
    {
        // Ground check first
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        anim.SetBool("IsGrounded", isGrounded);

        // Read horizontal input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        anim.SetBool("IsWalking", horizontalInput != 0f);

        // Set jump bool based on key held
        bool jumpPressed = Input.GetKey(jumpKey);
        anim.SetBool("IsJumping", jumpPressed);

        // Only launch upward velocity on initial jump press and grounded
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // anim.SetBool("IsJumping", true); // Not needed, since handled above by the key state
        }


        // 3) Crouch
        anim.SetBool("IsCrouching", Input.GetKey(crouchKey));

        // 4) Block
        anim.SetBool("IsBlocking", Input.GetKey(blockKey));

        // 5) Attacks
        if (Input.GetKeyDown(lightPunchKey))
            anim.SetTrigger("LightPunch");

        if (Input.GetKeyDown(heavyPunchKey))
            anim.SetTrigger("HeavyPunch");
    }

    void FixedUpdate()
    {
        // 6) Move in physics step
        Vector2 vel = rb.velocity;
        vel.x = horizontalInput * moveSpeed;
        rb.velocity = vel;

        // Flip character
        if (horizontalInput > 0)
            transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1.5f, 1.5f, 1f);
    }

    void LateUpdate()
    {
        // 7) Ground check & landing reset
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
        anim.SetBool("IsGrounded", isGrounded);

        // Once we land, clear the jumping flag so Idle/Walk can play
        if (isGrounded && !wasGrounded)
            anim.SetBool("IsJumping", false);
    }

    /// <summary>
    /// Call this from your damage logic to trigger the hit reaction.
    /// </summary>
    public void OnTakeDamage()
    {
        anim.SetTrigger("IsTakingDamage");
    }
}
