public class DashCommand : ICommand
{
    private readonly PlayerController _player;

    public DashCommand(PlayerController player)
    {
        _player = player;
    }

    public void Execute()
    {
        if (_player.isActiveAndEnabled) _player.Dash();
    }
}
