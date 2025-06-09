using UnityEngine;

public class enemy : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint; // Point where bullets will spawn
    [SerializeField] private float spreadAngle = 5f; // Maximum angle of spread in degrees

    private float nextShootTime;
    private GameManager gameManager;
    private Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the enemy tag
        gameObject.tag = "Enemy";

        // Find the GameManager
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }

        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }

        // If no fire point is assigned, use the enemy's position
        if (firePoint == null)
        {
            firePoint = transform;
        }

        // Set initial shoot time
        SetNextShootTime();
    }

    private void SetNextShootTime()
    {
        nextShootTime = Time.time + Random.Range(2f, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            // Check if it's time to shoot
            if (Time.time >= nextShootTime)
            {
                Shoot();
                SetNextShootTime();
            }

            // Make enemy face the player
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            directionToPlayer.y = 0; // Keep the enemy upright
            if (directionToPlayer != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && playerTransform != null)
        {
            // Calculate direction to player
            Vector3 directionToPlayer = (playerTransform.position - firePoint.position).normalized;
            directionToPlayer.y = 0; // Keep direction horizontal

            // Apply random spread only horizontally
            float randomSpread = Random.Range(-spreadAngle, spreadAngle);
            Quaternion spreadRotation = Quaternion.Euler(0, randomSpread, 0);
            Vector3 spreadDirection = spreadRotation * directionToPlayer;

            // Spawn bullet at fire point with spread direction
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(spreadDirection));
        }
        else
        {
            Debug.LogWarning("Bullet prefab not assigned to enemy or player not found!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy hit by: " + other.gameObject.name);
        // Check if the colliding object is a bullet
        if (other.CompareTag("Bullet"))
        {
            Die();
        }
    }

    public void Die()
    {
        // Decrease the number of enemies in GameManager
        if (gameManager != null)
        {
            gameManager.numberOfEnemies--;
        }
        else
        {
            Debug.LogError("GameManager not found when trying to die!");
        }

        // Destroy the enemy
        Destroy(gameObject);
    }
}
