namespace SpaceBattle.Lib;

using Hwdtech;
public class MacroCommand : ICommand
{
    public IEnumerable<ICommand> commands;
    public MacroCommand(IEnumerable<ICommand> commands)
    {
        this.commands = commands;
    }
    public void Execute()
    {
        foreach (ICommand command in commands)
        {
            command.Execute();
        }
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
        commandsName.ForEach(x => commands.Add
        (
            IoC.Resolve<ICommand>("Create." + x, UObject)
        ));

        return new MacroCommand(commands);
    }
}
