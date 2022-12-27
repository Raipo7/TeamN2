namespace SpaceBattle.Lib.Test;
using System.Collections.Generic;
using Moq;
using Xunit;
using Hwdtech;

public class TestEndMoveCommand
{

    public TestEndMoveCommand()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Mock<IStrategy> mockStrategy = new Mock<IStrategy>();

        Mock<SpaceBattle.Lib.ICommand> command = new Mock<SpaceBattle.Lib.ICommand>();
        command.Setup(x => x.Execute());
        mockStrategy.Setup(x => x.Execute(It.IsAny<object[]>())).Returns(command.Object);

        IoC.Resolve<ICommand>("IoC.Register", "Object.DeleteProperty", (object[] args) =>
        {
            return mockStrategy.Object.Execute(args);
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Object.SetProperty", (object[] args) =>
        {
            return mockStrategy.Object.Execute(args);
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Create.EmptyCommand", (object[] args) =>
        {
            return mockStrategy.Object.Execute();
        }).Execute();
    }
    [Fact]
    public void EndMoveCommandPositive()
    {
        Mock<ICommandEnbable> endable = new Mock<ICommandEnbable>();

        Mock<IUObject> UObject = new Mock<IUObject>();
        endable.Setup(x => x.UObject).Returns(UObject.Object).Verifiable();

        Dictionary<string, object> dict = new Dictionary<string, object>();
        endable.Setup(x => x.properties).Returns(dict).Verifiable();

        new EndMoveCommand(endable.Object).Execute();

        endable.Verify();
    }
}
