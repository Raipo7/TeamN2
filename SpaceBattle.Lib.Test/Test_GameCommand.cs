namespace SpaceBattle.Lib.Test;
using Xunit;
using Moq;
using Hwdtech;
using System.Collections.Generic;
public class TestGameCommand
{
    public object globalScope;
    public Queue<Lib.ICommand> queue;
    public IDictionary<string, object> gameItems;
    public int testInt = 0;

    public TestGameCommand()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        globalScope = scope;
        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        queue = new Queue<Lib.ICommand>();
        IoC.Resolve<ICommand>("IoC.Register", "Game.GetReceiver", (object[] args) =>
        {
            return new QueueReceiverAdapter(queue);
        }).Execute();
        gameItems = new Dictionary<string, object>();
        IoC.Resolve<ICommand>("IoC.Register", "Game.GetItems", (object[] args) =>
        {
            return gameItems;
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.GetTick", (object[] args) =>
        {
            return (object)100;
        }).Execute();
        Mock<IStrategy> strategy = new Mock<IStrategy>();
        strategy.Setup(x => x.Execute()).Callback(() => testInt++);
        IoC.Resolve<ICommand>("IoC.Register", "Exception.FindHandlerStrategy", (object[] args) =>
        {
            return strategy.Object;
        }).Execute();
    }

    [Fact]
    public void FullTestGameCommand()
    {
        Lib.ICommand emptyCommand = new EmptyCommand();
        var gameCmd = new GameCommand(globalScope);
        for (int i = 0; i < 20; i++)
        {
            queue.Enqueue(emptyCommand);
        }
        var err = new System.Exception();
        Mock<Lib.ICommand> errorCommand = new Mock<Lib.ICommand>();
        errorCommand.Setup(x => x.Execute()).Throws<System.Exception>(() => err);

        var a = IoC.Resolve<IReceiver>("Game.GetReceiver");

        queue.Enqueue(errorCommand.Object);
        gameCmd.Execute();

        Assert.Equal(1, testInt);
    }
}
