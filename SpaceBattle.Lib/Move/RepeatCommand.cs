namespace SpaceBattle.Lib;

using Hwdtech;

public class RepeatFromObjectCommand : ICommand
{
    IUObject obj;
    string commandName;

    public RepeatFromObjectCommand(IUObject obj, string commandName)
    {
        this.obj = obj;
        this.commandName = commandName;
    }
    public void Execute()
    {
        ICommand command = (ICommand)obj.GetProperty(commandName);
        if (command.GetType() != typeof(EmptyCommand))
        {
            command.Execute();
            IoC.Resolve<ICommand>("Queue.PushBack", IoC.Resolve<Queue<ICommand>>("Select.Queue"), this).Execute();
        }
    }
}
