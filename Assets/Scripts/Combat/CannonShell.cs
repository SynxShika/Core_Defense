using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CannonShell : MonoBehaviour
{
    [SerializeField] private float speed = 9f;
    [SerializeField] private int damage = 2;
    [SerializeField] private float splashRadius = 1.5f;
    [SerializeField] private float lifetime = 2f;

    private bool _exploded;

    private void Start()
    {
        GetComponent<Rigidbody2D>().SetVelocity(transform.up * speed);
        Invoke(nameof(Explode), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
            Explode();
    }

    private void Explode()
    {
        if (_exploded) return;
        _exploded = true;

        foreach (Collider2D hit in Physics2D.OverlapCircleAll(transform.position, splashRadius))
        {
            if (hit.CompareTag("Enemy") && hit.TryGetComponent(out Health health))
                health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f);
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}
