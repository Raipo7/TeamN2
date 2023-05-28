using Hwdtech;

namespace SpaceBattle.Lib;

public class SetGameObjectFuelCommand : ICommand
{
    IEnumerable<string> itemsId;
    IEnumerable<int> listFuel;

    public SetGameObjectFuelCommand(IEnumerable<string> itemsId, IEnumerable<int> listFuel)
    {
        this.itemsId = itemsId;
        this.listFuel = listFuel;
    }
    public void Execute()
    {
        IoC.Resolve<ICommand>("Game.FuelIterator.Setup", listFuel).Execute();
        foreach (var itemId in itemsId)
        {
            IoC.Resolve<ICommand>("Game.Items.SetProperty", itemId, "fuel", IoC.Resolve<int>("Game.FuelIterator.GetNext")).Execute();
        }
    }
}
