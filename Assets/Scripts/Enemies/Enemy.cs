using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Health))]
public abstract class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected int contactDamage = 1;
    [SerializeField] protected float attackInterval = 1f;
    [SerializeField] protected int scrapReward = 5; 

    protected Rigidbody2D Rb { get; private set; }
    protected Health SelfHealth { get; private set; }
    protected Transform Target { get; set; }

    public int ScrapReward => scrapReward;

    private float _nextAttackTime;

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        SelfHealth = GetComponent<Health>();
        SelfHealth.OnDied += Die;
    }

    protected virtual void Start()
    {

        GameObject core = GameObject.FindWithTag("Core");
        if (core != null) Target = core.transform;
    }

    protected virtual void OnDestroy()
    {
        if (SelfHealth != null) SelfHealth.OnDied -= Die;
    }

    protected virtual void FixedUpdate()
    {
        Rb.SetVelocity(Target != null ? GetDesiredVelocity() : Vector2.zero);
    }
    protected virtual Vector2 GetDesiredVelocity()
    {
        Vector2 toTarget = (Vector2)Target.position - Rb.position;
        return toTarget.normalized * moveSpeed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time < _nextAttackTime) return;
        if (!CanAttack(collision.collider)) return;
        if (!collision.collider.TryGetComponent(out Health targetHealth)) return;

        _nextAttackTime = Time.time + attackInterval;
        Attack(targetHealth);
    }

    protected virtual bool CanAttack(Collider2D other)
    {
        return other.CompareTag("Core") || other.CompareTag("Player") || other.CompareTag("Turret");
    }

    protected abstract void Attack(Health target);

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
