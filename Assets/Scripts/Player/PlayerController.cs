using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 18f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Aim & Shooting")]
    [SerializeField] private Transform aimPivot; 
    [SerializeField] private Transform firePoint;  
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float fireRate = 8f; 

    private Rigidbody2D _rb;
    private Vector2 _moveDir;
    private Vector2 _dashDir;
    private float _dashEndTime;
    private float _nextDashTime;
    private float _nextFireTime;

    private bool IsDashing => Time.time < _dashEndTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    public void Move(Vector2 direction)
    {
        _moveDir = Vector2.ClampMagnitude(direction, 1f); 
    }

    public void AimAt(Vector2 worldPoint)
    {
        Vector2 dir = worldPoint - (Vector2)aimPivot.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        aimPivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void Shoot()
    {
        if (Time.time < _nextFireTime) return;       
        _nextFireTime = Time.time + 1f / fireRate;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    public void Dash()
    {
        if (Time.time < _nextDashTime || _moveDir == Vector2.zero) return;
        _dashDir = _moveDir.normalized;
        _dashEndTime = Time.time + dashDuration;
        _nextDashTime = Time.time + dashCooldown;
    }


    private void FixedUpdate()
    {
        Vector2 velocity = IsDashing ? _dashDir * dashSpeed : _moveDir * moveSpeed;
        _rb.SetVelocity(velocity);
    }
}
