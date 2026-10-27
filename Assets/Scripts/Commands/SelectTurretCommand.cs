public class SelectTurretCommand : ICommand
{
    private readonly TurretPlacer _placer;
    private readonly int _index;

    public SelectTurretCommand(TurretPlacer placer, int index)
    {
        _placer = placer;
        _index = index;
    }

    public void Execute()
    {
        _placer.SelectTurret(_index);
    }
}
