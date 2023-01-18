namespace SpaceBattle.Lib;
using Hwdtech;

public class ExceptionThrower : ICommand
{
    IStrategy strategy;
    int commandHash, exceptionHash;
    public ExceptionThrower(params object[] args)
    {
        commandHash = ((ICommand)args[0]).GetType().GetHashCode();
        exceptionHash = ((Exception)args[1]).GetType().GetHashCode();
        strategy = (IStrategy)args[2];
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
