namespace SpaceBattle.Lib;

public class SendCommand : ICommand
{
    ISender sender;

    ICommand command;

    public SendCommand(ISender sender, ICommand command)
    {
        this.sender = sender;

        this.command = command;
    }

    public void Execute()
    {
        sender.Send(command);
    }
}
