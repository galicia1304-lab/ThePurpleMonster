using UnityEngine;

public class BossFireAttack : MonoBehaviour
{
    public GameObject firePrefab;      // your fire projectile prefab
    public Transform fireSpawnPoint;   // where the fire comes from
    public Transform player;           // player reference

    public float minFireInterval = 0.5f;
    public float maxFireInterval = 2f;

    private float fireTimer;

    void Start()
    {
        ResetFireTimer();
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            ShootFire();
            ResetFireTimer();
        }
    }

    void ShootFire()
    {
        if (firePrefab == null)
        {
            Debug.LogError("Fire prefab not assigned to BossFireAttack!");
            return;
        }
        if (player == null)
        {
            Debug.LogError("Player not assigned to BossFireAttack!");
            return;
        }

        GameObject fire = Instantiate(firePrefab, fireSpawnPoint.position, Quaternion.identity);

        FireProjectile projectile = fire.GetComponent<FireProjectile>();
        if (projectile != null)
        {
            projectile.SetTarget(player);
        }
        else
        {
            Debug.LogError("Your firePrefab is missing the FireProjectile script!");
        }
    }


    void ResetFireTimer()
    {
        fireTimer = Random.Range(minFireInterval, maxFireInterval);
    }
}





