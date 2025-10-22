using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public Transform startPoint;

    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float jumpForce = 10f;

    private bool facingRight = true;
    private bool isDead = false;

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

        // Move instantly with no delay
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // --- Flip Character ---
        if (moveInput > 0 && !facingRight)
            Flip();
        else if (moveInput < 0 && facingRight)
            Flip();

        // --- Set Animations ---
        animator.SetBool("isWalking", moveInput != 0 && !isRunning);
        animator.SetBool("isRunning", moveInput != 0 && isRunning);

        // --- Jump (no ground check, always jump when pressing space) ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }

        // Reset jump animation when falling down
        if (rb.linearVelocity.y == 0)
        {
            animator.SetBool("isJumping", false);
        }

        // --- Death ---
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        rb.linearVelocity = Vector2.zero;
        Invoke("Respawn", 1f);
    }

    void Respawn()
    {
        isDead = false;
        animator.SetBool("isDead", false);
        transform.position = startPoint.position;
        animator.Play("Idle");
    }
}
