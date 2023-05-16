using Hwdtech;

namespace SpaceBattle.Lib;

public class ThreadSoftStopCommand : ICommand
{

    ServerThread thread;

    Action action = () => { };
    public ThreadSoftStopCommand(ServerThread thread)
    {
        this.thread = thread;
    }
    public ThreadSoftStopCommand(ServerThread thread, Action action)
    {
        this.thread = thread;
        this.action = action;
    }
    public void Execute()
    {
        new UpdateBehaviourCommand(thread, () =>
        {
            if (!thread.ThreadIsEmpty())
            {
                thread.HandleCommand();
            }
            else
            {
                var threadId = IoC.Resolve<string>("Thread.GetIdByThread", thread);
                IoC.Resolve<ICommand>("Thread.SendCommand", threadId, IoC.Resolve<ICommand>("Thread.HardStopTheThread", threadId, action)).Execute();
            }
        }).Execute();
    }
}
