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
        var obj = IoC.Resolve<IUObject>("Game.Object.GetById", message.gameItemId);
        IoC.Resolve<ICommand>("Game.Object.SetPropertiesCommand", obj, message.properties).Execute();

        var newCommand = IoC.Resolve<ICommand>("Game.Command." + message.type, obj);
        
        IoC.Resolve<ICommand>("Game.SendCommand", message.gameId, newCommand).Execute();
    }
}
