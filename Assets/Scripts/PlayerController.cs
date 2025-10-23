using UnityEngine;
using UnityEngine.UI;

public class PlayerController2D : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public Transform startPoint;

    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float jumpForce = 10f;

    [Header("Player Stats")]
    public int maxLives = 3;
    private int currentLives;

    [Header("UI")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Coin Settings")]
    public int coinCount = 0;
    public Text coinText;

    private bool facingRight = true;
    private bool isDead = false;

    private bool invincible = false;           // Tracks temporary invincibility
    public float invincibleTime = 0.5f;        // Duration of invincibility after taking damage


    void Start()
    {
        currentLives = maxLives;
        UpdateHeartsUI();
        UpdateCoinUI();
    }

    void Update()
    {
        if (isDead) return;

        // Movement Input
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.A)) moveInput = -1f;
        if (Input.GetKey(KeyCode.D)) moveInput = 1f;

        // Walking & Running
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Flip Character
        if (moveInput > 0 && !facingRight) Flip();
        else if (moveInput < 0 && facingRight) Flip();

        // Set Animations
        animator.SetBool("isWalking", moveInput != 0 && !isRunning);
        animator.SetBool("isRunning", moveInput != 0 && isRunning);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }

        if (rb.linearVelocity.y == 0)
            animator.SetBool("isJumping", false);

        // Manual Death Test Key
        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // --- DAMAGE & LIVES ---
    public void TakeDamage()
    {
        if (isDead || invincible) return;   // Ignore damage if dead or invincible

        currentLives--;
        UpdateHeartsUI();

        if (currentLives <= 0)
        {
            Die();                           // Instantly die if lives reach 0
        }

    }


    void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("isDead", true);
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // Player can�t take damage or move now
        currentLives = 3; // immediately mark as dead
        UpdateHeartsUI();

        // Optional: respawn after a short delay
        Invoke(nameof(Respawn), 0.5f); // shorter delay
    }


    void Respawn()
    {
        isDead = false;
        animator.SetBool("isDead", false);
        animator.Play("Idle");

        transform.position = startPoint.position;
        rb.simulated = true;

        currentLives = maxLives;
        UpdateHeartsUI();
    }

    // --- TRIGGERS ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard"))
        {
            TakeDamage();
        }
        else if (collision.CompareTag("Heart"))
        {
            coinCount++;
            UpdateCoinUI();
            Destroy(collision.gameObject);
        }
    }

    // --- UI UPDATES ---
    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < currentLives ? fullHeart : emptyHeart;
        }
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + coinCount;
    }
}

