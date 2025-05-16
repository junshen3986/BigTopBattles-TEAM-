using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Key Bindings")]
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.W;
    public KeyCode crouchKey = KeyCode.S;
    public KeyCode blockKey = KeyCode.L;
    public KeyCode lightPunchKey = KeyCode.J;
    public KeyCode heavyPunchKey = KeyCode.K;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float jumpFallMultiplier = 1f;
    public Vector3 characterScale = new Vector3(1.5f, 1.5f, 1f);

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundRayDistance = 0.1f;

    [Header("Hitboxes & Delays")]
    public Collider2D lightPunchHitbox;
    public Collider2D crouchLightHitbox;
    public Collider2D heavyPunchHitbox;
    public float lightPunchDelay = 0f;
    public float crouchLightDelay = 0f;
    public float heavyPunchDelay = 0f;

    [Header("Damage & Stun")]
    public float stunDuration = 0.5f;

    [Header("Health")]
    public int maxHealth = 100;
    public Slider healthSlider;

    [Header("Facing")]
    public Transform opponent;

    // internals
    Rigidbody2D rb;
    Animator anim;
    Collider2D bodyCollider;
    float horizontalInput;
    bool isGrounded;
    bool hasJumped;
    float stunTimer;
    int currentHealth;

    // coroutines for delayed hitbox activation
    Coroutine lightPunchCoroutine;
    Coroutine heavyPunchCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bodyCollider = GetComponent<Collider2D>();
        rb.gravityScale = jumpFallMultiplier;

        // health UI init
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        // disable all hitboxes at start
        lightPunchHitbox.enabled = false;
        crouchLightHitbox.enabled = false;
        heavyPunchHitbox.enabled = false;
    }

    void Update()
    {
        // 0) stun lockout
        if (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;
            return;
        }

        // 1) ground check via raycast
        Bounds b = bodyCollider.bounds;
        Vector2 orig = new Vector2(b.center.x, b.min.y);
        var hit = Physics2D.Raycast(orig, Vector2.down, groundRayDistance, groundLayer);
        isGrounded = hit.collider != null;
        anim.SetBool("IsGrounded", isGrounded);

        // 2) reset jump on landing
        if (isGrounded && hasJumped)
        {
            hasJumped = false;
            anim.SetBool("IsJumping", false);
        }

        // 3) horizontal movement input
        float h = 0f;
        if (Input.GetKey(moveLeftKey)) h = -1f;
        if (Input.GetKey(moveRightKey)) h = +1f;
        horizontalInput = h;
        anim.SetBool("IsWalking", h != 0f);

        // 4) jump input
        if (Input.GetKeyDown(jumpKey) && isGrounded && !hasJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpFallMultiplier);
            hasJumped = true;
            anim.SetBool("IsJumping", true);
        }

        // 5) crouch
        bool isCrouching = Input.GetKey(crouchKey);
        anim.SetBool("IsCrouching", isCrouching);

        // 6) block
        anim.SetBool("IsBlocking", Input.GetKey(blockKey));

        // 7a) light punch with delay
        if (Input.GetKeyDown(lightPunchKey))
        {
            anim.SetTrigger("LightPunch1");
            if (lightPunchCoroutine != null) StopCoroutine(lightPunchCoroutine);
            lightPunchCoroutine = StartCoroutine(EnableHitboxAfterDelay(
                isCrouching ? crouchLightHitbox : lightPunchHitbox,
                isCrouching ? crouchLightDelay : lightPunchDelay
            ));
        }
        if (Input.GetKeyUp(lightPunchKey))
        {
            if (lightPunchCoroutine != null)
            {
                StopCoroutine(lightPunchCoroutine);
                lightPunchCoroutine = null;
            }
            lightPunchHitbox.enabled = false;
            crouchLightHitbox.enabled = false;
        }

        // 7b) heavy punch with delay
        if (Input.GetKeyDown(heavyPunchKey))
        {
            anim.SetTrigger("HeavyPunch1");
            if (heavyPunchCoroutine != null) StopCoroutine(heavyPunchCoroutine);
            heavyPunchCoroutine = StartCoroutine(EnableHitboxAfterDelay(
                heavyPunchHitbox, heavyPunchDelay
            ));
        }
        if (Input.GetKeyUp(heavyPunchKey))
        {
            if (heavyPunchCoroutine != null)
            {
                StopCoroutine(heavyPunchCoroutine);
                heavyPunchCoroutine = null;
            }
            heavyPunchHitbox.enabled = false;
        }

        // 8) face opponent
        if (opponent != null)
        {
            bool faceRight = opponent.position.x > transform.position.x;
            float sx = faceRight ? characterScale.x : -characterScale.x;
            transform.localScale = new Vector3(sx, characterScale.y, characterScale.z);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    IEnumerator EnableHitboxAfterDelay(Collider2D hitbox, float delay)
    {
        yield return new WaitForSeconds(delay);
        hitbox.enabled = true;
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        if (healthSlider != null) healthSlider.value = currentHealth;
        anim.SetTrigger("IsTakingDamage");
        stunTimer = stunDuration;
        // death now handled elsewhere
    }

    void OnDrawGizmosSelected()
    {
        if (bodyCollider != null)
        {
            Bounds bb = bodyCollider.bounds;
            Vector2 o = new Vector2(bb.center.x, bb.min.y);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(o, o + Vector2.down * groundRayDistance);
        }
    }
}
