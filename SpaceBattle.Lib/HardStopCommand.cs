namespace SpaceBattle.Lib;

public class HardStopCommand : ICommand
{
    private ServerThreadStrategy serverThread;
    public HardStopCommand(ServerThreadStrategy serverThread)
    {
        this.serverThread = serverThread;
    }
    public void Execute()
    {
        serverThread.Stop();
    }
}
