namespace SpaceBattle.Lib;

public class WorkWithGameQueue
{
    Queue<ICommand> commandQueue;
    public WorkWithGameQueue(Queue<ICommand> commandQueue)
    {
        this.commandQueue = commandQueue;
    }
    public void GameQueuePush(ICommand command)
    {
        commandQueue.Enqueue(command);
    }
    public ICommand GameQueuePop()
    {
        return (ICommand) commandQueue.Dequeue();
    }
}
