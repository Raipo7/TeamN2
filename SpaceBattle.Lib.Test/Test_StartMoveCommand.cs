namespace SpaceBattle.Lib.Test;
using Xunit;
using Moq;
using Hwdtech;
using System.Collections.Generic;

public class TestStartMoveCommand
{

    Queue<Lib.ICommand> queue = new Queue<Lib.ICommand>();

    public TestStartMoveCommand()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Mock<IStrategy> mockStrategy = new Mock<IStrategy>();

        Mock<SpaceBattle.Lib.ICommand> command = new Mock<SpaceBattle.Lib.ICommand>();
        command.Setup(x => x.Execute());
        mockStrategy.Setup(x => x.Execute(It.IsAny<object[]>())).Returns(command.Object);

        IoC.Resolve<ICommand>("IoC.Register", "Object.SetProperty", (object[] args) =>
        {
            return mockStrategy.Object.Execute(args);
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Create.MoveCommand", (object[] args) =>
        {
            return mockStrategy.Object.Execute(args);
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Queue.PushBack", (object[] args) =>
        {
            return new PushBackCommand<SpaceBattle.Lib.ICommand>((Queue<SpaceBattle.Lib.ICommand>)args[0], (SpaceBattle.Lib.ICommand)args[1]);
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Select.Queue", (object[] args) =>
        {
            return queue;
        }).Execute();

    }
    [Fact]
    public void StartMoveCommandPositive()
    {
        Assert.Empty(queue);

        Mock<ICommandStartable> startable = new Mock<ICommandStartable>();

        Mock<IUObject> UObject = new Mock<IUObject>();
        startable.Setup(x => x.UObject).Returns(UObject.Object);

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Velocity", It.IsAny<Vector>());
        startable.Setup(x => x.properties).Returns(dict);

        new StartMoveCommand(startable.Object).Execute();
        Assert.Single(queue);

    }
}
