namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class StopServerCommand : ICommand
{
    public void Execute()
    {   
        ConcurrentDictionary<int, object> myThreads = IoC.Resolve<ConcurrentDictionary<int, object>>("Thread.GetDictionary");
        foreach (int threadId in myThreads.Keys)
        {
            IoC.Resolve<ICommand>("Threads.HardStopTheThreads", threadId).Execute();
        }
    }
}
