using UnityEngine;

public class CannonTurret : Turret
{
    [SerializeField] private CannonShell shellPrefab;

    protected override void Fire()
    {
        Instantiate(shellPrefab, firePoint.position, head.rotation);
    }
}
