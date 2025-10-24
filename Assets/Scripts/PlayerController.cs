using UnityEngine;
using UnityEngine.UI;

public class PlayerController2D : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public Transform startPoint; // starting point of the level
    public Transform respawnPoint; // last checkpoint (can be same as start initially)

    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float jumpForce = 10f;

    [Header("Player Stats")]
    public int maxHearts = 3;   // hearts per life
    private int currentHearts;

    public int maxLives = 3;    // total lives before restart
    private int currentLives;

    [Header("UI")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Text livesText;

    [Header("Coin Settings")]
    public int coinCount = 0;
    public Text coinText;

    private bool facingRight = true;
    private bool isDead = false;
    private bool invincible = false;
    public float invincibleTime = 0.5f;


    void Start()
    {
        currentHearts = maxHearts;
        currentLives = maxLives;
        respawnPoint = startPoint; // initial respawn = start
        UpdateHeartsUI();
        UpdateLivesUI();
        UpdateCoinUI();
    }

    void Update()
    {
        if (isDead) return;

        float moveInput = 0f;
        if (Input.GetKey(KeyCode.A)) moveInput = -1f;
        if (Input.GetKey(KeyCode.D)) moveInput = 1f;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (moveInput > 0 && !facingRight) Flip();
        else if (moveInput < 0 && facingRight) Flip();

        animator.SetBool("isWalking", moveInput != 0 && !isRunning);
        animator.SetBool("isRunning", moveInput != 0 && isRunning);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }

        if (rb.linearVelocity.y == 0)
            animator.SetBool("isJumping", false);

        // Manual damage test key
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
        if (isDead || invincible) return;

        currentHearts--;
        UpdateHeartsUI();

        if (currentHearts <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    System.Collections.IEnumerator InvincibilityCoroutine()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("isDead", true);
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // Lose one life
        currentLives--;
        UpdateLivesUI();

        // Respawn after short delay
        Invoke(nameof(Respawn), 1.0f);
    }

    void Respawn()
    {
        isDead = false;
        animator.SetBool("isDead", false);
        animator.Play("Idle");

        rb.simulated = true;

        if (currentLives > 0)
        {
            // Respawn at the last checkpoint
            transform.position = respawnPoint.position;
            currentHearts = maxHearts;
            UpdateHeartsUI();
        }
        else
        {
            // Out of lives → restart from the beginning
            currentLives = maxLives;
            transform.position = startPoint.position;
            respawnPoint = startPoint;
            currentHearts = maxHearts;
            UpdateHeartsUI();
            UpdateLivesUI();
        }
    }

    // --- COLLISIONS ---
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
        else if (collision.CompareTag("Checkpoint"))
        {
            // Update the current respawn point
            respawnPoint = collision.transform;
        }
    }

    // --- UI ---
    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < currentHearts ? fullHeart : emptyHeart;
        }
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = ": " + currentLives;
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + coinCount;
    }
}