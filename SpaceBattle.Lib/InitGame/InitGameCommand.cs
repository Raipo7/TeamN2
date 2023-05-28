using Hwdtech;

namespace SpaceBattle.Lib;

public class InitGameCommand : ICommand
{

    private int gameObjectCount;
    public InitGameCommand(int gameObjectCount)
    {
        this.gameObjectCount = gameObjectCount;
    }

    public void Execute()
    {
        var itemsId = IoC.Resolve<IEnumerable<string>>("Game.CreateEmptyObjects", gameObjectCount);
        IoC.Resolve<ICommand>("Game.SetupObjectsProperties", itemsId).Execute();
    }
}
