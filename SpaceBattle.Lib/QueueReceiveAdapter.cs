namespace SpaceBattle.Lib;

public class QueueReceiverAdapter : IReceiver
{
    Queue<ICommand> queue;
    public QueueReceiverAdapter(Queue<ICommand> queue)
    {
        this.queue = queue;
    }
    public ICommand Receive()
    {
        return queue.Dequeue();
    }
    public bool isEmpty()
    {
        return queue.Count == 0;
    }
}
