using UnityEngine;

public class Brute : Enemy
{
    [SerializeField] private int turretDamageMultiplier = 3;

    protected override void Attack(Health target)
    {
        int damage = target.CompareTag("Turret") ? contactDamage * turretDamageMultiplier : contactDamage;
        target.TakeDamage(damage);
    }
}
