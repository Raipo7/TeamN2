namespace SpaceBattle.Lib;
using Hwdtech;

public class ExceptionFindHandlerStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        ICommand command = (ICommand)args[0];
        Exception exception = (Exception)args[1];

        int commandHash = command.GetType().GetHashCode();
        int exceptionHash = exception.GetType().GetHashCode();

        Dictionary<int, Dictionary<int, IStrategy>> exceptionTree = IoC.Resolve<Dictionary<int, Dictionary<int, IStrategy>>>("Exception.Get.Tree");

        Dictionary<int, IStrategy>? exceptionSubTree;

        if (!exceptionTree.TryGetValue(commandHash, out exceptionSubTree))
        {

            exceptionSubTree = IoC.Resolve<Dictionary<int, IStrategy>>("Exception.Get.NotFoundCommandSubTree");
        }

        IStrategy? exceptionHandler;

        if (!exceptionSubTree.TryGetValue(exceptionHash, out exceptionHandler))
        {
            return IoC.Resolve<IStrategy>("Exception.Get.NotFoundExcepetionHandler");
        }
        return exceptionHandler;
    }
}
