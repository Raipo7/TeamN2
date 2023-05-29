using System.Collections.Generic;
using Hwdtech;
using Xunit;

namespace SpaceBattle.Lib.Test;

public class Test_InitGameCommand
{
    public Dictionary<string, object> gameItemsDict;
    public Test_InitGameCommand()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        gameItemsDict = new Dictionary<string, object>();
        // ЧАСТЬ 1. Создание пустых объектов
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateEmptyObjects", (object[] args) =>
        {
            var gameObjectCount = (int)args[0];
            return new CreateObjectStrategy().Execute(gameObjectCount);

        }).Execute();
        // -- Генерация id для путсых объектов
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GenerateItemsId", (object[] args) =>
        {
            var gameObjectCount = (int)args[0];
            var listItemsId = new List<string>();
            for (int i = 1; i <= gameObjectCount; i++)
            {
                listItemsId.Add(i.ToString());
            }
            return listItemsId;
        }).Execute();
        // -- Создание и добавление пустого объекта в словарь по id
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateItem", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var itemId = (string)args[0];
                gameItemsDict.Add(itemId, new Dictionary<string, object>());
            });
        }).Execute();
        // ЧАСТЬ 2. Установка свойств для объектов
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.SetupObjectsProperties", (object[] args) =>
        {
            var itemsId = (IEnumerable<string>)args[0];
            var cmdList = new List<Lib.ICommand>();
            int[] pos = new int[6] { 1, 2, 4, 5, 6, 8 };
            Queue<int> queuePos = new Queue<int>();
            foreach (int i in pos)
            {
                queuePos.Enqueue(i);
            }
            var startCoords = new Vector(0, 0);
            cmdList.Add(new SetGameObjectPositionCommand(itemsId, startCoords, queuePos));

            IEnumerable<int> fuel = new List<int>() { 100, 100, 100, 100, 100, 100 };
            cmdList.Add(new SetGameObjectFuelCommand(itemsId, fuel));
            return new MacroCommand(cmdList);
        }).Execute();

        var gameIteratorDict = new Dictionary<string, object>();
        // -- Задание начальных свойств итератору позиций
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.PosIterator.Setup", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var startCoords = (Vector)args[0];
                var queue = (Queue<int>)args[1];

                gameIteratorDict.Add("posIterator", new GetPosIterator(startCoords, queue));
            });
        }).Execute();
        // -- Выброс следующего значения из итератора позиций
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.PosIterator.GetNext", (object[] args) =>
        {
            var posIterator = (GetPosIterator)gameIteratorDict["posIterator"];
            posIterator.MoveNext();
            return posIterator.Current;
        }).Execute();

        // -- Задание начальных свойств итератору топлива
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.FuelIterator.Setup", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var listFuel = (IEnumerable<int>)args[0];

                gameIteratorDict.Add("fuelIterator", new GetFuelIterator(listFuel));
            });
        }).Execute();
        // -- Выброс следующего значения из итератора топлива
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.FuelIterator.GetNext", (object[] args) =>
        {

            var fuelIterator = (GetFuelIterator)gameIteratorDict["fuelIterator"];
            fuelIterator.MoveNext();
            return fuelIterator.Current;
        }).Execute();
        // -- Установка свойств игровым объектам
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Items.SetProperty", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var itemId = (string)args[0];
                var propName = (string)args[1];
                var prop = args[2];

                var itemPropDict = (IDictionary<string, object>)gameItemsDict[itemId];
                itemPropDict[propName] = prop;
            });
        }).Execute();
    }

    [Fact]
    public void TestPosIterator()
    {
        var startCoords = new Vector(0, 0);

        int[] pos = new int[8] {0, 1, 2, 4, 5, 6, 8, 32 };
        Queue<int> queuePos = new Queue<int>();
        foreach (int i in pos)
        {
            queuePos.Enqueue(i);
        }
        var iterator = new GetPosIterator(new Vector(0, 0), queuePos);
        iterator.MoveNext();
        Assert.Equal(new Vector(0, 0), iterator.Current);
        iterator.MoveNext();
        Assert.Equal(new Vector(1, 0), iterator.Current);
        iterator.MoveNext();
        Assert.Equal(new Vector(1, 1), iterator.Current);
        iterator.MoveNext();
        Assert.Equal(new Vector(-1, 1), iterator.Current);
        iterator.MoveNext();
        Assert.Equal(new Vector(-1, 0), iterator.Current);
        iterator.MoveNext();
        Assert.Equal(new Vector(-1, -1), iterator.Current);
        iterator.MoveNext();
        Assert.Equal(new Vector(1, -1), iterator.Current);
        iterator.MoveNext();
        Assert.Equal(new Vector(1, 3), iterator.Current);

        Assert.Throws<System.NotImplementedException>(() => iterator.Reset());
    }
    [Fact]
    public void TestFuelIterator()
    {
        int[] fuel = new int[3] {10, 20, 30};
        var fuleList = new List<int>() {10, 20, 30};

        var iterator = new GetFuelIterator(fuleList);
        iterator.MoveNext();
        Assert.Equal(10, iterator.Current);
        iterator.MoveNext();
        Assert.Equal(20, iterator.Current);
        iterator.MoveNext();
        Assert.Equal(30, iterator.Current);
        
        Assert.Throws<System.NotImplementedException>(() => iterator.Reset());
    }
    [Fact]
    public void TestInitGameCommand()
    {
        new InitGameCommand(6).Execute();

        var item2 = (Dictionary<string, object>)gameItemsDict["2"];
        var item6 = (Dictionary<string, object>)gameItemsDict["6"];

        Assert.Equal(new Vector(1, 1), item2["position"]);
        Assert.Equal(new Vector(1, -1), item6["position"]);
        Assert.Equal(100, item2["fuel"]);
        Assert.Equal(100, item6["fuel"]);
    }
}
