using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class SenderAdapter : ISender
{
    public BlockingCollection<ICommand> queue;

    public SenderAdapter(BlockingCollection<ICommand> queue)
    {
        this.queue = queue;
    }

    public void Send(ICommand cmd)
    {
        queue.Add(cmd);
    }
}
