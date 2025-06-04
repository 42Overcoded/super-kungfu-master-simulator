using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HandBulletInteractions : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private LayerMask handLayerMask = -1;

    void Start()
    {
        if (GetComponent<Collider>() == null)
        {
            Debug.LogError("HandsManager requires a Collider component to detect bullet collisions!");
        }
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            BounceBullet(other);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            BounceBullet(collision.collider);
        }
    }

    private void BounceBullet(Collider bulletCollider)
    {
        Rigidbody bulletRb = bulletCollider.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            // Calculate bounce direction (reflect the velocity)
            Vector3 incomingDirection = bulletRb.linearVelocity.normalized;
            Vector3 normal = (bulletCollider.transform.position - transform.position).normalized;
            Vector3 bounceDirection = Vector3.Reflect(incomingDirection, normal);

            // Apply bounce force
            bulletRb.linearVelocity = bounceDirection * bounceForce;

            // Optional: Add some random variation to make it more interesting
            bulletRb.linearVelocity += Random.insideUnitSphere * 2f;
        }
    }
}
