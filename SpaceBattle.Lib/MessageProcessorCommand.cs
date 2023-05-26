namespace SpaceBattle.Lib;
using System.Collections.Concurrent;
using Hwdtech;

public class MessageProcessorCommand : ICommand
{
    private ConcurrentQueue<IMessage> messageQueue;

    public MessageProcessorCommand(ConcurrentQueue<IMessage> messageQueue)
    {
        this.messageQueue = messageQueue;
    }

    public void Execute()
    {
        while (messageQueue.TryDequeue(out var message))
        {
            var interpretationCommand = new InterpretationCommand(message);
            IoC.Resolve<ICommand>("Game.SendCommand", message.gameId, interpretationCommand).Execute();
        }
    }
}