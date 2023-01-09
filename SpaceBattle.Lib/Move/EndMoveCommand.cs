namespace SpaceBattle.Lib;

using Hwdtech;

public class EndMoveCommand : ICommand
{
    private ICommandEnbable obj;

    public EndMoveCommand(ICommandEnbable obj)
    {
        this.obj = obj;
    }
    public void Execute()
    {
        obj.properties.ToList().ForEach(x => IoC.Resolve<ICommand>("Object.DeleteProperty", obj.UObject, x.Key).Execute());

        IoC.Resolve<ICommand>("Object.SetProperty", obj.UObject, "RepeatMoveCommand", IoC.Resolve<ICommand>("Create.EmptyCommand")).Execute();
    }
}
