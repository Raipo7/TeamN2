namespace SpaceBattle.Lib;
using System.Threading;
using System.Collections.Concurrent;

public class ServerThreadStrategy : IStrategy
{
    private IStrategy strategy;
    private BlockingCollection<ICommand> commandQueue;
    private Thread currentThread;
    private bool isRunning;

    public ServerThreadStrategy(IStrategy strategy, BlockingCollection<ICommand> commandQueue)
    {
        this.strategy = strategy;
        this.commandQueue = commandQueue;
        this.currentThread = new Thread(CurrentThreadExecuteCommands);
        isRunning = true;
    }

    public void ChangeBehaviour(IStrategy strategy)
    {
        if (Thread.CurrentThread == currentThread)
        {   
            this.strategy = strategy;
        }
    }

    public void Stop()
    {
        if (Thread.CurrentThread == currentThread)
        {   
            isRunning = false;
        }
    }

    public object Execute(params object[] args)
    {
        return currentThread;
    }

    private void CurrentThreadExecuteCommands()
    {
        while (isRunning)
        {
            ICommand command = (ICommand)strategy.Execute(commandQueue);
            command.Execute();
        }
    }
}
