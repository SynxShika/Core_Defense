using UnityEngine;

public class SellTurretCommand : IUndoableCommand
{
    private readonly TurretPlacer _placer;
    private readonly Turret _prefab;
    private readonly Vector2 _cell;
    private Turret _targetTurret;

    public bool Succeeded { get; private set; }

    public SellTurretCommand(TurretPlacer placer, Turret targetTurret)
    {
        _placer = placer;
        _targetTurret = targetTurret;
        _prefab = targetTurret.SourcePrefab;
        _cell = targetTurret.transform.position;
    }

    public void Execute()
    {
        Succeeded = _targetTurret != null;
        if (!Succeeded) return;

        _placer.RemoveTurret(_targetTurret, true);
        _targetTurret = null;
    }

    public void Undo()
    {
        _targetTurret = _placer.PlaceTurret(_prefab, _cell);
    }

    public override string ToString()
    {
        return $"Sell {_prefab.DisplayName} at {_cell}";
    }
}
