namespace SpaceBattle.Lib.Test;
using Xunit;
using Moq;
using Hwdtech;
using System.Collections.Generic;

public class TestRepeatMoveCommand
{

    Queue<Lib.ICommand> queue = new Queue<Lib.ICommand>();

    public TestRepeatMoveCommand()
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

        IoC.Resolve<ICommand>("IoC.Register", "Object.DeleteProperty", (object[] args) =>
        {
            return mockStrategy.Object.Execute(args);
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Create.EmptyCommand", (object[] args) =>
        {
            return new EmptyCommand();
        }).Execute();
    }

    [Fact]
    public void RepeatMoveCommandPositive()
    {
        Mock<ICommandStartable> startable = new Mock<ICommandStartable>();
        Mock<IUObject> UObject = new Mock<IUObject>();

        startable.Setup(x => x.UObject).Returns(UObject.Object);

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Velocity", new Vector(1, 2));

        Mock<IMovable> movable = new Mock<IMovable>();
        movable.Setup(x => x.Position).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>()));
        movable.Setup(x => x.Velocity).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>()));
        MoveCommand moveCommand = new MoveCommand(movable.Object);
        Assert.Equal(moveCommand.GetType(), typeof(MoveCommand));
        startable.Setup(x => x.properties).Returns(dict);


        Assert.Equal(0, queue.Count); //начало - пустая очередь

        UObject.Setup(x => x.GetProperty("MoveCommand")).Returns(moveCommand);
        new StartMoveCommand(startable.Object).Execute(); //в очередь 1 команда начала движения
        Assert.Equal(1, queue.Count);

        queue.Dequeue().Execute(); //вызов команды начала движения и у объекта появляется UObkect.GetProperty("MoveCommand")
        Assert.Equal(0, queue.Count);

        new RepeatFromObjectCommand(startable.Object.UObject, "MoveCommand").Execute();//в очередь 1 команда повторения движения
        queue.Dequeue().Execute(); //вызов команды повторения и ее возврат в очередь
        Assert.Equal(1, queue.Count);
        queue.Dequeue().Execute();
        Assert.Equal(1, queue.Count);
        queue.Dequeue().Execute();
        Assert.Equal(1, queue.Count);


        Mock<ICommandEnbable> endable = new Mock<ICommandEnbable>();
        endable.Setup(x => x.UObject).Returns(UObject.Object);
        endable.Setup(x => x.properties).Returns(dict);

        UObject.Setup(x => x.GetProperty("MoveCommand")).Returns(new EmptyCommand());

        new EndMoveCommand(endable.Object); //переназначение в объекте UObkect.GetProperty("MoveCommand") на пустую команду
        queue.Dequeue().Execute(); // при попытке вызова повторяющаейся команды отслеживается пустая и повтор останавлвается
        Assert.Equal(0, queue.Count);
    }
}
