using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f; // Speed of the bullet
    [SerializeField] private float lifetime = 15f; // How long the bullet exists before being destroyed
    [SerializeField] private bool _isActive = true; // Whether the bullet is active or not

    public float damage = 10f; // Damage dealt by the bullet

    public Vector3 direction;

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
        if (!_isActive) return;
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public void FireBullet()
    {
        direction = transform.forward;
        _isActive = true;
    }

    public void FireBullet(Vector3 newDirection)
    {
        direction = newDirection.normalized;
        _isActive = true;
    }

    public void StopBullet()
    {
        _isActive = false;
    }
}