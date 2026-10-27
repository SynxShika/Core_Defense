public class RedoCommand : ICommand
{
    private readonly CommandInvoker _invoker;

    public RedoCommand(CommandInvoker invoker)
    {
        _invoker = invoker;
    }

    public void Execute()
    {
        _invoker.Redo();
    }
}
