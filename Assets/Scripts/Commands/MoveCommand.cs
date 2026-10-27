using UnityEngine;

public class MoveCommand : ICommand
{
    private readonly PlayerController _player;

    public Vector2 Direction { get; set; }

    public MoveCommand(PlayerController player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.Move(Direction);
    }
}
