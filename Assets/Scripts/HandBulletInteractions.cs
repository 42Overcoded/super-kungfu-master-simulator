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
        if (other.CompareTag("Bullet"))
        {
            BounceBullet(other);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BounceBullet(collision.collider);
        }
    }

    private void BounceBullet(Collider bulletCollider)
    {
        Bullet bulletScript = bulletCollider.GetComponent<Bullet>();
        Vector3 incomingDirection = bulletScript.direction;

        if (bulletScript != null)
        {
            // Optionally, you can reset the bullet's state or properties
            bulletScript.FireBullet(incomingDirection * -1); // Reverse the direction
        }
    }
}
