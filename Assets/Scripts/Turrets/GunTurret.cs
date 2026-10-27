using UnityEngine;

public class GunTurret : Turret
{
    [SerializeField] private Bullet bulletPrefab;

    protected override void Fire()
    {
        Instantiate(bulletPrefab, firePoint.position, head.rotation);
    }
}
