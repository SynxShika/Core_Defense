public class Runner : Enemy
{
    protected override void Attack(Health target)
    {
        target.TakeDamage(contactDamage);
        Remove(false);
    }
}
