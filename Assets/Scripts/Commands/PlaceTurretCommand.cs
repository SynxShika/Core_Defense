using UnityEngine;

public class PlaceTurretCommand : IUndoableCommand
{
    private readonly TurretPlacer _placer;
    private readonly Turret _prefab;
    private readonly Vector2 _cell;
    private Turret _placedTurret;

    public bool Succeeded => _placedTurret != null;

    public PlaceTurretCommand(TurretPlacer placer, Turret prefab, Vector2 cell)
    {
        _placer = placer;
        _prefab = prefab;
        _cell = cell;
    }

    public void Execute()
    {
        _placedTurret = _placer.PlaceTurret(_prefab, _cell);
    }

    public void Undo()
    {
        if (_placedTurret == null) return;

        _placer.RemoveTurret(_placedTurret, true);
        _placedTurret = null;
    }

    public override string ToString()
    {
        return $"Place {_prefab.DisplayName} at {_cell}";
    }
}
