namespace SpaceBattle.Lib;

public class ExceptionFindHandler : ICommand
{   
    private ICommand command;
    private Exception ex;
    public ExceptionFindHandler(ICommand command, Exception ex)
    {
        this.command = command;
        this.ex = ex;
    }
    public void Execute()
    {
        string logFileName = "error.log";
        string errorMessage = $"[{DateTime.Now}] Error in command '{command.GetType().Name}': {ex.Message}";

        using (StreamWriter writer = new StreamWriter(logFileName, true))
        {
            writer.WriteLine(errorMessage);
        }
    }
}