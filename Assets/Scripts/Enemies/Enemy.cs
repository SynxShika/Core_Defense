using System;
using System.Collections.Generic;
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

    public static readonly List<Enemy> Active = new List<Enemy>();

    public int ScrapReward => scrapReward;
    public float MoveSpeed => moveSpeed;
    public Vector2 Position => Rb.position;

    public event Action<Enemy> OnRemoved;

    private float _nextAttackTime;
    private bool _removed;
    private bool _hasSteering;
    private Vector2 _steeringVelocity;

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        SelfHealth = GetComponent<Health>();
        SelfHealth.OnDied += HandleDied;
    }

    protected virtual void Start()
    {
        CoreObjective core = GameManager.Instance.Core;
        if (core != null) Target = core.transform;
    }

    protected virtual void OnEnable()
    {
        Active.Add(this);
    }

    protected virtual void OnDisable()
    {
        Active.Remove(this);
        _hasSteering = false;
    }

    public void SetSteeringVelocity(Vector2 velocity)
    {
        _steeringVelocity = velocity;
        _hasSteering = true;
    }

    protected virtual void OnDestroy()
    {
        if (SelfHealth != null) SelfHealth.OnDied -= HandleDied;
    }

    protected virtual void FixedUpdate()
    {
        Rb.SetVelocity(Target != null ? GetDesiredVelocity() : Vector2.zero);
    }

    protected virtual Vector2 GetDesiredVelocity()
    {
        if (_hasSteering) return _steeringVelocity;

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

    protected void Remove(bool giveReward)
    {
        if (_removed) return;
        _removed = true;

        if (giveReward && GameManager.HasInstance)
            GameManager.Instance.AddScrap(scrapReward);

        OnRemoved?.Invoke(this);
        Destroy(gameObject);
    }

    private void HandleDied()
    {
        Remove(true);
    }
}
