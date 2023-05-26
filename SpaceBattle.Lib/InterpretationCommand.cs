namespace SpaceBattle.Lib;
using System.Collections.Concurrent;
using Hwdtech;

public class InterpretationCommand : ICommand
{
    private IMessage message;

    public InterpretationCommand(IMessage message)
    {
        this.message = message;
    }

    public void Execute(){
        if (message.type == "StartMove") IoC.Resolve<ICommand>("Game.StartMoveCommand", IoC.Resolve<IUObject>("Game.GetObjectById", message.gameItemId), message.properties).Execute();
        else if (message.type == "StartRotate") IoC.Resolve<ICommand>("Game.StartRotateCommand", IoC.Resolve<IUObject>("Game.GetObjectById", message.gameItemId), message.properties).Execute();
        else if (message.type == "StopMove") IoC.Resolve<ICommand>("Game.StopMoveCommand", IoC.Resolve<IUObject>("Game.GetObjectById", message.gameItemId), message.properties).Execute();
        else if (message.type == "Shoot") IoC.Resolve<ICommand>("Game.ShootCommand", IoC.Resolve<IUObject>("Game.GetObjectById", message.gameItemId), message.properties).Execute();
    }
}