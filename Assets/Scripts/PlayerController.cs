using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 startPosition;
    private bool isGrounded = true;
    private bool isDead = false;    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");
        bool fight = Input.GetKeyDown(KeyCode.F);

        // Movement
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // Flip character
        if (move != 0)
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);

        // Set animation states
        anim.SetBool("isRunning", move != 0);
        anim.SetBool("isJumping", !isGrounded);
        anim.SetBool("isFighting", fight);

        // Jump
        if (jump && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        // Death trigger (press K)
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetBool("isDead", true);

        // Stop movement immediately
        rb.linearVelocity = Vector2.zero;

        // Wait for 2 seconds (death animation) then respawn
        Invoke(nameof(Respawn), 2f);
    }

    void Respawn()
    {
        // Reset death state
        isDead = false;

        // Move back to start
        transform.position = startPosition;

        // Reset animator states
        anim.SetBool("isDead", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isFighting", false);

        // Force Idle state
        anim.Play("Idle");  // <<-- This line ensures it goes back to idle
    }
}
