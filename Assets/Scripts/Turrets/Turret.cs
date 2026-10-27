using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Turret : MonoBehaviour
{
    [Header("Turret")]
    [SerializeField] private string displayName = "Turret";
    [SerializeField] private int cost = 20;
    [SerializeField] protected float range = 4f;
    [SerializeField] protected float fireRate = 2f;
    [SerializeField] private float retargetInterval = 0.2f;

    [Header("Parts")]
    [SerializeField] protected Transform head;
    [SerializeField] protected Transform firePoint;

    public string DisplayName => displayName;
    public int Cost => cost;

    public event Action<Turret> OnTurretRemoved;

    protected Transform CurrentTarget { get; private set; }

    private Health _health;
    private float _nextFireTime;
    private float _nextRetargetTime;
    private bool _removed;

    protected virtual void Awake()
    {
        _health = GetComponent<Health>();
        _health.OnDied += HandleDied;
    }

    protected virtual void OnDestroy()
    {
        if (_health != null) _health.OnDied -= HandleDied;
    }

    protected virtual void Update()
    {
        if (Time.time >= _nextRetargetTime)
        {
            _nextRetargetTime = Time.time + retargetInterval;
            CurrentTarget = FindNearestEnemy();
        }

        if (CurrentTarget == null) return;

        AimAt(CurrentTarget.position);

        if (Time.time < _nextFireTime) return;
        _nextFireTime = Time.time + 1f / fireRate;
        Fire();
    }

    protected abstract void Fire();

    public void Remove()
    {
        if (_removed) return;
        _removed = true;

        OnTurretRemoved?.Invoke(this);
        Destroy(gameObject);
    }

    private void AimAt(Vector2 worldPoint)
    {
        Vector2 dir = worldPoint - (Vector2)head.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        head.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private Transform FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        Transform nearest = null;
        float bestDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            float distance = ((Vector2)hit.transform.position - (Vector2)transform.position).sqrMagnitude;
            if (distance < bestDistance)
            {
                bestDistance = distance;
                nearest = hit.transform;
            }
        }

        return nearest;
    }

    private void HandleDied()
    {
        Remove();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
