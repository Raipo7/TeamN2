namespace SpaceBattle.Lib.Test;
using Moq;
using System.Collections.Generic;
using Hwdtech;
using Xunit;

public class WorkWithGameTest
{
    public WorkWithGameTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.GetTick", (object[] args) =>
        {
            return (object)100;
        }).Execute();
    }
    [Fact]
    public void Test_GameObjectsDeleteGet()
    {
        var gameItemId = "item1";
        var obj = new object();
        var objects = new Dictionary<string, object>()
        {
            { gameItemId, obj }
        };

        var workWithGameObjects = new WorkWithGameObjects(objects);
        var result = workWithGameObjects.GameObjectGet(gameItemId);
        Assert.Equal(obj, result);

        workWithGameObjects.GameObjectDelete(gameItemId);
        Assert.DoesNotContain(gameItemId, objects.Keys);
    }
    [Fact]
    public void Test_GameQueuePushAndPop()
    {
        var commandMock = new Mock<Lib.ICommand>();
        var commandQueueMock = new Mock<Queue<Lib.ICommand>>();

        var workWithGameQueue = new WorkWithGameQueue(commandQueueMock.Object);
        workWithGameQueue.GameQueuePush(commandMock.Object);
        Assert.True(commandQueueMock.Object.Contains(commandMock.Object));

        var command = workWithGameQueue.GameQueuePop();
        Assert.Equal(command, commandMock.Object);
    }
}
