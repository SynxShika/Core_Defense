using UnityEngine;

public class RequestPlaceTurretCommand : ICommand
{
    private readonly TurretPlacer _placer;
    private readonly CommandInvoker _invoker;

    public RequestPlaceTurretCommand(TurretPlacer placer, CommandInvoker invoker)
    {
        _placer = placer;
        _invoker = invoker;
    }

    public void Execute()
    {
        if (!_placer.TryGetPlacementAtMouse(out Turret prefab, out Vector2 cell)) return;
        _invoker.ExecuteUndoable(new PlaceTurretCommand(_placer, prefab, cell));
    }
}
