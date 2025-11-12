using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damageAmount = 20;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            BossHealth boss = other.GetComponent<BossHealth>();
            if (boss != null)
            {
                boss.TakeDamage(damageAmount);
            }
        }
    }
}

