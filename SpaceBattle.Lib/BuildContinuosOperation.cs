namespace SpaceBattle.Lib;
using Hwdtech;

public class BuildContinuosOperationStrategy : IStrategy 
{
    public object Execute(params object[] args){
        string dependencyName = (string)args[0];
        IUObject obj = (IUObject)args[1];

        List <ICommand> commandsList = new List <ICommand>(); 
        ICommand macroCommand = IoC.Resolve<ICommand>("MacroCommandCreate", dependencyName, obj);
        Queue<ICommand> queue = IoC.Resolve<Queue<ICommand>>("Select.Queue");
        ICommand PushCommand = IoC.Resolve<ICommand>("Queue.PushBack", queue, macroCommand);
        commandsList.Add(macroCommand);
        commandsList.Add(PushCommand);
        return new MacroCommand(commandsList);
    }
}