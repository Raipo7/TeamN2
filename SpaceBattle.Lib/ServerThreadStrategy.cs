namespace SpaceBattle.Lib;
using System.Threading;
using System.Collections.Concurrent;

public class ServerThreadStrategy : IStrategy
{
    private Action strategy;
    private BlockingCollection<ICommand> commandQueue;
    private Thread currentThread;
    private bool isRunning = true;

    public ServerThreadStrategy(BlockingCollection<ICommand> commandQueue)
    {   
        this.commandQueue = commandQueue;
        this.strategy = () => {
            var cmd = commandQueue.Take();
            cmd.Execute();
        };
        this.currentThread = new Thread(() => {
            while(isRunning) {
                strategy();
                }
        });
    }

    public void ChangeBehaviour(Action strategy)
    {
        if (Thread.CurrentThread == currentThread)
        {   
            this.strategy = strategy;
        }
    }

    internal void Stop()
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
}
