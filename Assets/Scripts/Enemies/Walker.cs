
public class Walker : Enemy
{
    protected override void Attack(Health target)
    {
        target.TakeDamage(contactDamage);
    }
}
