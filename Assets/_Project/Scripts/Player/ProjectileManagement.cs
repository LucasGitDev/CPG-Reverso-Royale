using UnityEngine;

public class ProjectileManagement : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Set the projectile to destroy itself after 2 seconds
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        rb.velocity = rb.transform.up * 10f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
