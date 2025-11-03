using Unity.VisualScripting;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    private Vector3 direction;
    public float gravity = -9.8f;
    public float strength = 5f;

    private void Update()
    {
        // Jump / move up
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            direction = Vector3.up * strength;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                direction = Vector3.up * strength;
        }

        // Apply gravity
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    // Detect collisions with hazards
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            // Tell GameManager to remove a life
            GameManager.Instance.LoseLife();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            GameManager.Instance.LoseLife();
        }
    }
}