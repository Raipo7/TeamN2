namespace SpaceBattle.Lib;
using Hwdtech;

public class ExceptionTreeAddHandlerCommand : ICommand
{
    private int commandHash;
    private int exceptionHash;
    private IStrategy strategy;

    public ExceptionTreeAddHandlerCommand(ICommand command, Exception exception, IStrategy strategy)
    {
        this.commandHash = command.GetType().GetHashCode();
        this.exceptionHash = exception.GetType().GetHashCode();
        this.strategy = strategy;
    }
    public void Execute()
    {
        Dictionary<int, Dictionary<int, IStrategy>> exceptionTree = IoC.Resolve<Dictionary<int, Dictionary<int, IStrategy>>>("Exception.Get.Tree");
        Dictionary<int, IStrategy> exceptionSubTree;
        if (!exceptionTree.ContainsKey(commandHash))
        {
            exceptionTree[commandHash] = new Dictionary<int, IStrategy>();
        }
        exceptionSubTree = exceptionTree[commandHash];
        exceptionSubTree[exceptionHash] = strategy;
    }
}
