namespace SpaceBattle.Lib.Test;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using Hwdtech;

public class TestMacroCommand {
    public TestMacroCommand() {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Object.MoveAndRotate.Get.CommandsName", (object[] args) =>
        {
            return new List<string>() {"MoveCommand", "RotateCommand"};
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Create.MacroCommand", (object[] args) =>
        {
            return new CreateMacroCommandStategy().Execute(args);
        }).Execute();

    }
    [Fact]
    public void MacroCommandTestPositive() {
        Mock<IUObject> UObject = new Mock<IUObject>();
        UObject.Setup(x => x.GetProperty("Position")).Returns(new Vector(0, 0));
        UObject.Setup(x => x.GetProperty("Velocity")).Returns(new Vector(1, 1));

        Mock<SpaceBattle.Lib.ICommand> mockMoveCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockMoveCommand.Setup(x => x.Execute()).Verifiable();

        Mock<SpaceBattle.Lib.ICommand> mockRotateCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockRotateCommand.Setup(x => x.Execute()).Verifiable();

        UObject.Setup(x => x.GetProperty("MoveCommand")).Returns(mockMoveCommand.Object);
        UObject.Setup(x => x.GetProperty("RotateCommand")).Returns(mockRotateCommand.Object);

        IoC.Resolve<MacroCommand>("Create.MacroCommand", UObject.Object, "Object.MoveAndRotate").Execute();

        mockMoveCommand.Verify();
        mockRotateCommand.Verify();
    }
}