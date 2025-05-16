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
    [Tooltip("Scales both your initial jump force and the Rigidbody2D.gravityScale")]
    public float jumpFallMultiplier = 1f;
    public Vector3 characterScale = new Vector3(1.5f, 1.5f, 1f);
    public Transform opponent;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;

    [Header("Damage & Stun")]
    public float stunDuration = 0.5f;    // how long to disable inputs
    private float stunTimer;             // counts down when stunned

    Rigidbody2D rb;
    Animator anim;
    float horizontalInput;
    bool isGrounded;
    bool hasJumped;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Apply the multiplier to gravity
        rb.gravityScale = jumpFallMultiplier;
    }

    void Update()
    {
            // 0) Stun timer: if >0, reduce and skip input
            if (stunTimer > 0f)
            {
                stunTimer -= Time.deltaTime;
                return;
            }


        // 1) Ground check
            bool touchingGround = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
        isGrounded = touchingGround && rb.velocity.y <= 0.001f;
        anim.SetBool("IsGrounded", isGrounded);

        // 2) If grounded, reset jump
        if (isGrounded && hasJumped)
        {
            hasJumped = false;
            anim.SetBool("IsJumping", false);
        }

        // 3) Horizontal movement / walking
        horizontalInput = Input.GetAxisRaw("Horizontal");
        anim.SetBool("IsWalking", horizontalInput != 0f);

        // 4) Jump input: only if grounded and not already jumped
        if (Input.GetKeyDown(jumpKey) && isGrounded && !hasJumped)
        {
            // Scale the jump force by the same multiplier
            float actualJump = jumpForce * jumpFallMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, actualJump);

            hasJumped = true;
            anim.SetBool("IsJumping", true);
        }

        // 5) Crouch
        anim.SetBool("IsCrouching", Input.GetKey(crouchKey));

        // 6) Block
        anim.SetBool("IsBlocking", Input.GetKey(blockKey));

        // 7) Attacks
        if (Input.GetKeyDown(lightPunchKey)) anim.SetTrigger("LightPunch1");
        if (Input.GetKeyDown(heavyPunchKey)) anim.SetTrigger("HeavyPunch1");

       /* if (Input.GetKeyDown(heavyPunchKey))
        {
            if (anim.GetBool("IsCrouching"))
                anim.SetTrigger("CrouchHeavyPunch");
            else
                anim.SetTrigger("HeavyPunch1");
        } */

        if (opponent != null)
        {
            bool shouldFaceRight = opponent.position.x > transform.position.x;
            float x = shouldFaceRight ? characterScale.x : -characterScale.x;
            transform.localScale = new Vector3(x, characterScale.y, characterScale.z);
        }
    }

    void FixedUpdate()
    {
        // Horizontal movement
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

    }

    // Visualize your ground‐check radius in the Editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    /// <summary>
    /// Call this to play your hit reaction.
    /// </summary>

    /// <summary>
    /// Called by Hitbox when this character is hit.
    /// </summary>
    public void TakeDamage(int amount)
    {
        // (Optionally track health here)
        anim.SetTrigger("IsTakingDamage");
        stunTimer = stunDuration;
    }

}
