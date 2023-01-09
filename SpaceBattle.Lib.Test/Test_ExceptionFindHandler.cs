namespace SpaceBattle.Lib.Test;
using Hwdtech;
using Moq;
using Xunit;
using System.Collections.Generic;

public class TestExceptionFindHandler
{

    public TestExceptionFindHandler()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Mock<IStrategy> mockStrategy = new Mock<IStrategy>();
        mockStrategy.Setup(x => x.Execute()).Returns("it's strategy");

        Mock<IStrategy> mockDefaultStrategy = new Mock<IStrategy>();
        mockDefaultStrategy.Setup(x => x.Execute()).Returns("it's default strategy");

        Dictionary<int, Dictionary<int, IStrategy>> dict = new() { { 11111, new Dictionary<int, IStrategy>() { { 22222, mockStrategy.Object } } } };
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Get.Tree", (object[] args) =>
        {
            return dict;
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Get.DefaultExcepetionHandler", (object[] args) =>
        {
            return mockDefaultStrategy.Object;
        }).Execute();
    }

    [Fact]
    public void ExceptionFindHandlerTestPositive()
    {
        Mock<SpaceBattle.Lib.ICommand> mockCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockCommand.Setup(x => x.GetHashCode()).Returns(11111);
        Mock<System.Exception> mockException = new Mock<System.Exception>();
        mockException.Setup(x => x.GetHashCode()).Returns(22222);

        IStrategy strategy = (IStrategy)new ExceptionFindHandlerStrategy().Execute(mockCommand.Object, mockException.Object);


        Assert.Equal(strategy.Execute(), "it's strategy");
    }
    [Fact]
    public void ExceptionFindHandlerStepCommandTestNegative()
    {
        Mock<SpaceBattle.Lib.ICommand> mockCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockCommand.Setup(x => x.GetHashCode()).Returns(33333);
        Mock<System.Exception> mockException = new Mock<System.Exception>();
        mockException.Setup(x => x.GetHashCode()).Returns(22222);

        IStrategy strategy = (IStrategy)new ExceptionFindHandlerStrategy().Execute(mockCommand.Object, mockException.Object);


        Assert.Equal(strategy.Execute(), "it's default strategy");
    }
    [Fact]
    public void ExceptionFindHandlerStepExceptionTestNegative()
    {
        Mock<SpaceBattle.Lib.ICommand> mockCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockCommand.Setup(x => x.GetHashCode()).Returns(11111);
        Mock<System.Exception> mockException = new Mock<System.Exception>();
        mockException.Setup(x => x.GetHashCode()).Returns(33333);

        IStrategy strategy = (IStrategy)new ExceptionFindHandlerStrategy().Execute(mockCommand.Object, mockException.Object);


        Assert.Equal(strategy.Execute(), "it's default strategy");
    }
}
