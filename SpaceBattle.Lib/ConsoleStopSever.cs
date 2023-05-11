namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class StopServerCommand : ICommand
{
    private ConcurrentDictionary<int, object> myThreads;
    public StopServerCommand(ConcurrentDictionary<int, object> myThreads)
    {
        this.myThreads = myThreads;
    }
    public void Execute()
    {
        foreach (int threadId in myThreads.Keys)
        {
            IoC.Resolve<ICommand>("Threads.HardStopTheThreads", threadId);
        }
        foreach (Thread thread in myThreads.Values)
        {
            thread.Join();
        }
    }
}
