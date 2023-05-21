namespace SpaceBattle.Lib.Test;
using Hwdtech;
using Xunit;
using System.Collections.Generic;
using Moq;

public class Test_ExceptionTreeAddHandler
{
    public Test_ExceptionTreeAddHandler()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Dictionary<int, Dictionary<int, IStrategy>> dict = new();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Get.Tree", (object[] args) =>
        {
            return dict;
        }).Execute();
    }

    [Fact]
    public void ExceptionTreeAddHandlerTest()
    {
        Dictionary<int, Dictionary<int, IStrategy>> dict = IoC.Resolve<Dictionary<int, Dictionary<int, IStrategy>>>("Exception.Get.Tree");
        Mock<SpaceBattle.Lib.ICommand> mockCommand = new Mock<Lib.ICommand>();
        Mock<System.Exception> mockException1 = new Mock<System.Exception>();
        IStrategy strategy = new CollisionCheckStrategy();

        new ExceptionTreeAddHandlerCommand(mockCommand.Object, mockException1.Object, strategy).Execute();

        int treeStep1 = mockCommand.Object.GetType().GetHashCode();
        int treeStep2 = mockException1.Object.GetType().GetHashCode();
        Assert.Equal(typeof(CollisionCheckStrategy), dict[treeStep1][treeStep2].GetType());


        Mock<System.ArgumentException> mockException2 = new Mock<System.ArgumentException>();

        new ExceptionTreeAddHandlerCommand(mockCommand.Object, mockException2.Object, strategy).Execute();
        treeStep2 = mockException2.Object.GetType().GetHashCode();
        Assert.Equal(typeof(CollisionCheckStrategy), dict[treeStep1][treeStep2].GetType());
    }
}
