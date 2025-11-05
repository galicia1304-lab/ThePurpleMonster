using UnityEngine;

public class BossFireAttack : MonoBehaviour
{
    [Header("Fire Settings")]
    public Sprite fireSprite;
    public Transform fireSpawnPoint;
    public Vector3 fireScale = new Vector3(0.5f, 0.5f, 1f);
    public float fireSpeed = 5f;

    [Header("Firing Settings")]
    public float fireInterval = 0.4f;        // Time between shots
    public int projectilesPerShot = 3;       // Number of fireballs per shot
    public float verticalSpread = 1f;        // Vertical distance between fireballs

    private float fireTimer;

    void Start()
    {
        fireTimer = fireInterval;
    }

    void Update()
    {
        if (fireSprite == null || fireSpawnPoint == null)
            return;

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            ShootFire();
            fireTimer = fireInterval; // reset timer
        }
    }

    void ShootFire()
    {
        for (int i = 0; i < projectilesPerShot; i++)
        {
            GameObject fire = new GameObject("FireProjectile");

            // Spread fire vertically so they don't overlap
            float yOffset = (i - (projectilesPerShot - 1) / 2f) * verticalSpread;
            fire.transform.position = fireSpawnPoint.position + new Vector3(0, yOffset, 0);
            fire.transform.localScale = fireScale;

            SpriteRenderer sr = fire.AddComponent<SpriteRenderer>();
            sr.sprite = fireSprite;

            FireProjectile proj = fire.AddComponent<FireProjectile>();
            proj.speed = fireSpeed;

            // Set direction mostly left with slight angle so projectiles are separated
            float angleY = yOffset * 0.2f; // small angle based on vertical offset
            proj.SetDirection(new Vector3(-1f, angleY, 0f));
        }
    }
}


