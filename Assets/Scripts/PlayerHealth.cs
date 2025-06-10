using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private Collider playerCollider;
    [SerializeField]
    private float _maxHealth = 100f;

    private float _currentHealth;
    private GameManager _gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerCollider == null)
        {
            Debug.LogError("Player Collider is not assigned!");
        }
        _gameManager = FindFirstObjectByType<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }

        _currentHealth = _maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
                Destroy(other.gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        _gameManager.EndGame();
        playerCollider.enabled = false;
    }
}
