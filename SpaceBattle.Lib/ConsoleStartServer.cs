namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class StartServerCommand : ICommand
{
    private int numOfThread;
    public StartServerCommand(int numOfThread)
    {
        this.numOfThread = numOfThread;
    }
    public void Execute()
    {
        for (int i = 0; i < numOfThread; i++)
        {
            int thread_id = IoC.Resolve<int>("Thread.Create");
            IoC.Resolve<ICommand>("Thread.Start", thread_id).Execute();
        }
    }
}
