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
    public Image[] hearts; // Assign heart sprites in order in the Inspector
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Coin Settings")]
    public int coinCount = 0;
    public Text coinText; // Optional: UI Text to show coin count

    private bool facingRight = true;
    private bool isDead = false;

    void Start()
    {
        currentLives = maxLives;
        UpdateHeartsUI();
        UpdateCoinUI();
    }

    void Update()
    {
        if (isDead) return;

        // --- Movement Input ---
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.A))
            moveInput = -1f;
        if (Input.GetKey(KeyCode.D))
            moveInput = 1f;

        // --- Walking & Running ---
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // --- Flip Character ---
        if (moveInput > 0 && !facingRight)
            Flip();
        else if (moveInput < 0 && facingRight)
            Flip();

        // --- Set Animations ---
        animator.SetBool("isWalking", moveInput != 0 && !isRunning);
        animator.SetBool("isRunning", moveInput != 0 && isRunning);

        // --- Jump ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }

        if (rb.linearVelocity.y == 0)
            animator.SetBool("isJumping", false);

        // --- Manual Death Test Key ---
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
        if (isDead) return; // <<< prevent hit while dead

        currentLives--;
        UpdateHeartsUI();

        if (currentLives <= 3)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("isHit");
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        // Clear the hit trigger to avoid animation conflicts
        animator.ResetTrigger("isHit");

        animator.SetBool("isDead", true);
        rb.linearVelocity = Vector2.zero; // linearVelocity is deprecated in new Unity versions
        rb.simulated = false;

        Invoke(nameof(Respawn), 1f);
    }

    void Respawn()
    {
        // Reset player
        isDead = false;
        animator.SetBool("isDead", false);
        animator.Play("Idle");

        // Move back to start point
        transform.position = startPoint.position;

        // Re-enable physics and controls
        rb.simulated = true;

        // Reset lives if needed
        currentLives = maxLives;
        UpdateHeartsUI();
    }


    // --- COIN PICKUP ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Thorn"))
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
            if (i < currentLives)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + coinCount;
    }
}

