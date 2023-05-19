namespace SpaceBattle.Lib;

using Hwdtech;

public class ServerThread
{
    bool stop = false;
    Thread thread;
    IReceiver queue;

    Action strategy;

    public ServerThread(IReceiver receiver)
    {
        this.queue = receiver;

        this.strategy = () =>
        {
            HandleCommand();
        };

        thread = new Thread(() =>
        {
            while (!stop)
            {
                strategy();
            }
        });
    }
    public void Start()
    {
        thread.Start();
    }

    internal void Stop()
    {
        stop = true;
    }

    public bool ThreadIsEmpty()
    {
        return queue.isEmpty();
    }

    public bool ThreadEqual(Thread secondThread)
    {
        return this.thread == secondThread;
    }
    internal void UpdateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;
    }

    internal void HandleCommand()
    {
        var cmd = queue.Receive();
        try
        {
            cmd.Execute();
        }
        catch (Exception err)
        {
            var exceptinHandlerStrategy = IoC.Resolve<IStrategy>("Exception.FindHandlerStrategy", cmd, err);
            exceptinHandlerStrategy.Execute();
        }
    }
}
