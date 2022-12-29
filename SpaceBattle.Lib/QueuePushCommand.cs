namespace SpaceBattle.Lib;

public class PushBackCommand<T> : ICommand
{
    private Queue<T> queue;
    private T obj;
    public PushBackCommand(Queue<T> queue, T obj)
    {
        this.queue = queue;
        this.obj = obj;
    }
    public void Execute()
    {
        queue.Enqueue(obj);
    }
}
