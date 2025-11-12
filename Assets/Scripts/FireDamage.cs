using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public int damage = 20;  // How much damage the fire does

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            BossHealth boss = other.GetComponent<BossHealth>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            // Optional: destroy fire after hitting the boss
            Destroy(gameObject);
        }
    }
}

