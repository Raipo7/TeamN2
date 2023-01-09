namespace SpaceBattle.Lib;
using Hwdtech;

public class ExceptionFindHandlerStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        ICommand command = (ICommand)args[0];
        Exception exception = (Exception)args[1];

        int commandHash = command.GetHashCode();
        int exceptionHash = exception.GetHashCode();
        Dictionary<int, Dictionary<int, IStrategy>> exceptionTree = IoC.Resolve<Dictionary<int, Dictionary<int, IStrategy>>>("Exception.Get.Tree");

        if (exceptionTree.ContainsKey(commandHash)) //в дереве нашлась команда
        {
            if (exceptionTree[commandHash].ContainsKey(exceptionHash)) //в дереве нашлась ошибка
            {
                return exceptionTree[commandHash][exceptionHash];
            }
        }
        return IoC.Resolve<IStrategy>("Exception.Get.DefaultExcepetionHandler"); //стандартный обработчик исключения
    }
}
