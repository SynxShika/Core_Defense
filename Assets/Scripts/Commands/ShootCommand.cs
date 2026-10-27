public class ShootCommand : ICommand
{
    private readonly PlayerController _player;

    public ShootCommand(PlayerController player)
    {
        _player = player;
    }

    public void Execute()
    {
        if (_player.isActiveAndEnabled) _player.Shoot();
    }
}
