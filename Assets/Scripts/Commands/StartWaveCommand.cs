public class StartWaveCommand : ICommand
{
    public void Execute()
    {
        GameManager.Instance.SkipBuildPhase();
    }
}
