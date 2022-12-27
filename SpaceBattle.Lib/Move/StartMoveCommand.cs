namespace SpaceBattle.Lib;

using System.Collections.Generic;
using Hwdtech;

public class StartMoveCommand : ICommand
{

    private ICommandStartable obj;

    public StartMoveCommand(ICommandStartable obj)
    {
        this.obj = obj;
    }
    public void Execute()
    {
        obj.properties.ToList().ForEach(x => IoC.Resolve<ICommand>("Object.SetProperty", obj.UObject, x, x.Value).Execute());
        ICommand moveCommand = IoC.Resolve<ICommand>("Create.MoveCommand", obj.UObject);
        IoC.Resolve<ICommand>("Object.SetProperty", obj.UObject, "MoveCommand", moveCommand);
        IoC.Resolve<ICommand>("Queue.PushBack", IoC.Resolve<Queue<ICommand>>("Select.Queue"), moveCommand).Execute();
    }
}
