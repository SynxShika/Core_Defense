public class UndoCommand : ICommand
{
    private readonly CommandInvoker _invoker;

    public UndoCommand(CommandInvoker invoker)
    {
        _invoker = invoker;
    }

    public void Execute()
    {
        _invoker.Undo();
    }
}
