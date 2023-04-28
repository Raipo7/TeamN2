namespace SpaceBattle.Lib;

public class StartServerThreadCommand : ICommand
{
    private ServerThreadStrategy serverThread;

    public StartServerThreadCommand(ServerThreadStrategy serverThread)
    {
        this.serverThread = serverThread;
    }

    public void Execute()
    {
        var thread = (Thread)serverThread.Execute();
        thread.Start();
    }
}
