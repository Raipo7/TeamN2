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
    int startRotateSent = 0;
    int stopMoveSent = 0;
    int shootSent = 0;
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

        IoC.Resolve<ICommand>("IoC.Register", "Game.GetObjectById", (object[] args) =>
        {
            var newobj = new Mock<IUObject>();
            objectWasGet++;
            return newobj.Object;
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.StartMoveCommand", (object[] args) =>
        {
            return new ActionCommand( () => {startMoveSent++;});
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.StopMoveCommand", (object[] args) =>
        {
            return new ActionCommand( () => {stopMoveSent++;});
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.StartRotateCommand", (object[] args) =>
        {
            return new ActionCommand( () => {startRotateSent++;});
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.ShootCommand", (object[] args) =>
        {
            return new ActionCommand( () => {shootSent++;});
        }).Execute();

    }
    [Fact]
    public void Test_SendInterpretationCommandInGameQueue()
    {
        var messageQueue = new ConcurrentQueue<IMessage>();

        var message1 = new Mock<IMessage>();
        var message2 = new Mock<IMessage>();

        message1.Setup(m => m.gameId).Returns("123");
        message2.Setup(m => m.gameId).Returns("456");
        messageQueue.Enqueue(message1.Object);
        messageQueue.Enqueue(message2.Object);

        var messageProcessorCommand = new MessageProcessorCommand(messageQueue);

        messageProcessorCommand.Execute();

        Assert.Equal(2, commandsWereSent);
    }
    [Fact]
    public void Test_CreateInterpretationCommand()
    {
        var messageMock1 = new Mock<IMessage>();
        messageMock1.Setup(m => m.type).Returns("StartMove");
        var messageMock2 = new Mock<IMessage>();
        messageMock2.Setup(m => m.type).Returns("StartRotate");
        var messageMock3 = new Mock<IMessage>();
        messageMock3.Setup(m => m.type).Returns("StopMove");
        var messageMock4 = new Mock<IMessage>();
        messageMock4.Setup(m => m.type).Returns("Shoot");

        var interpretationCommand1 = new InterpretationCommand(messageMock1.Object);
        var interpretationCommand2 = new InterpretationCommand(messageMock2.Object);
        var interpretationCommand3 = new InterpretationCommand(messageMock3.Object);
        var interpretationCommand4 = new InterpretationCommand(messageMock4.Object);

        interpretationCommand1.Execute();
        interpretationCommand2.Execute();
        interpretationCommand3.Execute();
        interpretationCommand4.Execute();

        Assert.Equal(4, objectWasGet);
        Assert.Equal(1, startMoveSent);
        Assert.Equal(1, stopMoveSent);
        Assert.Equal(1, startRotateSent);
        Assert.Equal(1, shootSent);
    }
}