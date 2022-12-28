namespace SpaceBattle.Lib;

using Hwdtech;

public class RepeatFromObjectCommand : ICommand
{
    private IUObject obj;
    private string commandName;
    private ICommand moveCommand;

    public RepeatFromObjectCommand(IUObject obj, string commandName)
    {
        this.obj = obj;
        this.commandName = commandName;
        this.moveCommand = (ICommand)obj.GetProperty(commandName);
        obj.SetProperty(commandName, this);
    }
    public void Execute()
    {
        moveCommand.Execute();
        IoC.Resolve<ICommand>("Queue.PushBack", IoC.Resolve<Queue<ICommand>>("Select.Queue"), obj.GetProperty(commandName)).Execute();
    }
}
