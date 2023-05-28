using Hwdtech;

namespace SpaceBattle.Lib;

public class SetGameObjectPositionCommand : ICommand
{
    IEnumerable<string> itemsId;
    Queue<int> queuePos;
    Vector startCoords;
    public SetGameObjectPositionCommand(IEnumerable<string> itemsId, Vector startCoords, Queue<int> queuePos)
    {
        this.itemsId = itemsId;
        this.queuePos = queuePos;
        this.startCoords = startCoords;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("Game.PosIterator.Setup", startCoords, queuePos).Execute();
        foreach (var itemId in itemsId)
        {
            IoC.Resolve<ICommand>("Game.Items.SetProperty", itemId, "position", IoC.Resolve<Vector>("Game.PosIterator.GetNext")).Execute();
        }

    }
}
