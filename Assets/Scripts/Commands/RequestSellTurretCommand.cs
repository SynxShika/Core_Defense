public class RequestSellTurretCommand : ICommand
{
    private readonly TurretPlacer _placer;
    private readonly CommandInvoker _invoker;

    public RequestSellTurretCommand(TurretPlacer placer, CommandInvoker invoker)
    {
        _placer = placer;
        _invoker = invoker;
    }

    public void Execute()
    {
        Turret turret = _placer.GetTurretAtMouse();
        if (turret == null) return;
        _invoker.ExecuteUndoable(new SellTurretCommand(_placer, turret));
    }
}
