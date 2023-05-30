namespace SpaceBattle.Lib;

public class GameQueuePopStrategy : IStrategy
{
    public object Execute(params object[] param)
    {
        Queue<ICommand> commandQueue = (Queue<ICommand>) param[0];
        return (ICommand) commandQueue.Dequeue();
    }
}
