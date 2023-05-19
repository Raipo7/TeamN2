using Hwdtech;

namespace SpaceBattle.Lib;

public class ThreadHardStopCommand : ICommand
{

    ServerThread thread;
    Action action = () => { };
    public ThreadHardStopCommand(ServerThread thread)
    {
        this.thread = thread;
    }
    public ThreadHardStopCommand(ServerThread thread, Action action)
    {
        this.thread = thread;
        this.action = action;
    }
    public void Execute()
    {
        if (thread.ThreadEqual(Thread.CurrentThread))
        {

            thread.Stop();
        }
        else
        {
            throw new Exception();
        }
        action();
    }
}
