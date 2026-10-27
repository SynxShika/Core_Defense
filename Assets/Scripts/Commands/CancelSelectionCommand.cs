public class CancelSelectionCommand : ICommand
{
    private readonly TurretPlacer _placer;

    public CancelSelectionCommand(TurretPlacer placer)
    {
        _placer = placer;
    }

    public void Execute()
    {
        _placer.ClearSelection();
    }
}
