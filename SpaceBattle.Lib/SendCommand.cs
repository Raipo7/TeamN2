namespace SpaceBattle.Lib;
using System.Collections.Concurrent;

public class SendCommand : ISender
 {
    private BlockingCollection<ICommand> queue;
    public SendCommand(BlockingCollection<ICommand> queue) 
    {
        this.queue = queue;
    }

    public void Send(ICommand command) 
    {
        queue.Add(command);
    }
}
