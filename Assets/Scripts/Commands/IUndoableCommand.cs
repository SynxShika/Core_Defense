public interface IUndoableCommand : ICommand
{
    bool Succeeded { get; }
    void Undo();
}
