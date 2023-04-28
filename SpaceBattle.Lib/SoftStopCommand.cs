namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class SoftStopCommand : ICommand
{
    private ServerThreadStrategy serverThread;
    private BlockingCollection<ICommand> queue;
    public SoftStopCommand(ServerThreadStrategy serverThread, BlockingCollection<ICommand> queue)
    {
        this.serverThread = serverThread;
        this.queue = queue;
    }
    public void Execute()
    {
        //ICommand hardStopCommand = IoC.Resolve<ICommand>("HardStopCommand", serverThread);
        IoC.Resolve<ICommand>("SendCommand", queue, IoC.Resolve<ICommand>("HardStopCommand", serverThread));
    }
}