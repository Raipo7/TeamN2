namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class StartServerCommand : ICommand
{
    private int numOfThread;
    private ConcurrentDictionary<int, object> myThreads;
    public StartServerCommand(int numOfThread, ConcurrentDictionary<int, object> myThreads)
    {
        this.numOfThread = numOfThread;
        this.myThreads = myThreads;
    }
    public void Execute()
    {
        for (int i = 0; i < numOfThread; i++)
        {
            BlockingCollection<ICommand> commandQueue = new BlockingCollection<ICommand>();
            var thread = IoC.Resolve<object>("Threads.Create", commandQueue);
            IoC.Resolve<ICommand>("Threads.Start", thread);
            myThreads[i] = thread;
        }
    }
}
