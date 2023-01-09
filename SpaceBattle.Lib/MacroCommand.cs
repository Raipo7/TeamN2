namespace SpaceBattle.Lib;

using Hwdtech;
public class MacroCommand : ICommand
{
    public List<ICommand> commands;
    public MacroCommand(List<ICommand> commands)
    {
        this.commands = commands;
    }
    public void Execute()
    {
        commands.ForEach(x => x.Execute());
    }
}

public class CreateMacroCommandStategy : IStrategy
{

    public object Execute(params object[] args)
    {
        IUObject UObject = (IUObject)args[0];
        string macroName = (string)args[1] + ".Get.CommandsName";
        List<string> commandsName = IoC.Resolve<List<string>>(macroName);
        List<ICommand> commands = new();
        commandsName.ForEach(x => commands.Add((ICommand)UObject.GetProperty(x)));

        return new MacroCommand(commands);
    }
}
