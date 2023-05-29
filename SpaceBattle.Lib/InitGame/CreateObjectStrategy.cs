using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateObjectStrategy : IStrategy
{
    public object Execute(params object[] arg)
    {
        var gameObjectCount = (int)arg[0];
        var itemsId = IoC.Resolve<IEnumerable<string>>("Game.GenerateItemsId", gameObjectCount);

        foreach (var itemId in itemsId)
        {
            IoC.Resolve<ICommand>("Game.CreateItem", itemId).Execute();
        }
        return itemsId;
    }
}
