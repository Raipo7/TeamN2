namespace SpaceBattle.Lib.Test;
using Xunit;
using Moq;
using Hwdtech;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

public class Test_MessageProcessorCommand
{   
    int commandsWereSent = 0;
    int startMoveSent = 0;
    int propertiesWereSet = 0;
    int objectWasGet = 0;
    public Test_MessageProcessorCommand()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.SendCommand", (object[] args) =>
        {
            return new ActionCommand( () => {commandsWereSent++;});
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Object.GetById", (object[] args) =>
        {
            var newobj = new Mock<IUObject>();
            objectWasGet++;
            return newobj.Object;
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.StartMove", (object[] args) =>
        {
            startMoveSent++;
            return new EmptyCommand();
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Object.SetPropertiesCommand", (object[] args) =>
        {
            return new ActionCommand( () => {propertiesWereSet++;});
        }).Execute();
    }
    [Fact]
    public void Test_CreateInterpretationCommand()
    {
        var mockMessage = new Mock<IMessage>();
        var mockUObject = new Mock<IUObject>();
        var mockCommand = new Mock<ICommand>();
        var mockSender = new Mock<ISender>();

        var gameId = "123";
        var gameItemId = "o123";
        var properties = new Dictionary<string, object> { { "InititalVelocity", 2 }, { "InititialHP", 50 } };
        var type = "StartMove";

        mockMessage.SetupGet(m => m.gameId).Returns(gameId);
        mockMessage.SetupGet(m => m.gameItemId).Returns(gameItemId);
        mockMessage.SetupGet(m => m.properties).Returns(properties);
        mockMessage.SetupGet(m => m.type).Returns(type);

        var interpretationCommand = new InterpretationCommand(mockMessage.Object);
        interpretationCommand.Execute();

        Assert.Equal(1, propertiesWereSet);
        Assert.Equal(1, startMoveSent);
        Assert.Equal(1, commandsWereSent);
        Assert.Equal(1, objectWasGet);
    }
}
