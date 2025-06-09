using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f; // Speed of the bullet
    [SerializeField] private float lifetime = 15f; // How long the bullet exists before being destroyed

    public float damage = 10f; // Damage dealt by the bullet

    private Vector3 direction;

    private void Start()
    {
        // Verify bullet setup
        gameObject.tag = "Bullet";

        // Store the initial direction (without Y component)
        direction = transform.forward;
        direction.y = 0;
        direction = direction.normalized;

        // Destroy the bullet after lifetime seconds
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move the bullet in the stored direction
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If we hit anything else (except other bullets), destroy the bullet
        if (!other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
} 