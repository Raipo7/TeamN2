namespace SpaceBattle.Lib.Test;
using Xunit;
using Moq;
using Hwdtech;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

public class Testovka
{
    public object IoCdependency() {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        Dictionary<string, ServerThread> dictThreads = new Dictionary<string, ServerThread>();
        Dictionary<string, ISender> dictSenders = new Dictionary<string, ISender>();

        IoC.Resolve<ICommand>("IoC.Register", "Threads.GetThreadById", (object[] args) => {
            var threadId = (string) args[0];
            return dictThreads[threadId];

        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Threads.GetIdByThreas", (object[] args) => {
            var thread = (ServerThread) args[0];

            foreach(var (key, value) in dictThreads) {
                if(value == thread) {
                    return key;
                }
            }
            throw new System.Exception();

        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Threads.GetSenderById", (object[] args) => {
            var threadId = (string) args[0];
            return dictSenders[threadId];

        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Threads.CreateAndStartThread", (object[] args) => {

            if (args.Length == 1) {
                var threadId = (string) args[0];

                BlockingCollection<Lib.ICommand> queue = new BlockingCollection<Lib.ICommand>();

                ISender sender = new SenderAdapter(queue);
                IReceiver receiver = new ReceiverAdapter(queue);
                ServerThread thread = new ServerThread(receiver);

                thread.Start();

                dictSenders.Add(threadId, sender);
                dictThreads.Add(threadId, thread);
                return thread;
            }
            else if (args.Length == 2) {
                var threadId = (string) args[0];
                var action = (Action) args[1];

                BlockingCollection<Lib.ICommand> queue = new BlockingCollection<Lib.ICommand>();

                ISender sender = new SenderAdapter(queue);
                IReceiver receiver = new ReceiverAdapter(queue);
                ServerThread thread = new ServerThread(receiver);
                
                queue.Add(new UpdateBehaviourCommand(thread, action));

                thread.Start();

                dictSenders.Add(threadId, sender);
                dictThreads.Add(threadId, thread);
                return thread;
            }
            else {
                throw new Exception();
            }
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Threads.SendCommand", (object[] args) => {
            if (args.Length == 2) {
                var threadId = (string) args[0];
                var commandsList = (IEnumerable<Lib.ICommand>) args[1];
                var sender = IoC.Resolve<ISender>("Threads.GetSenderById", threadId);
                return new SendCommand(threadId, commandsList);
            }
            else if (args.Length == 3) {
                var threadId = (string) args[0];           
                var commands = (IEnumerable<Lib.ICommand>) args[2];
                var action = (Action) args[3];

                var sender = IoC.Resolve<ISender>("Threads.GetSenderById", threadId);
                var thread = IoC.Resolve<ServerThread>("Threads.GetThreadById", threadId);

                var commandsList = new List<Lib.ICommand>();
                commandsList.Add(new UpdateBehaviourCommand(thread, action));
                commandsList.Add(new SendCommand(threadId, commandsList));
                return (Lib.ICommand) new MacroCommand(commandsList);
            }
            else {
                throw new Exception();
            }
        }).Execute();
        
        IoC.Resolve<ICommand>("IoC.Register", "Threads.HardStopTheThread", (object[] args) => {
            if (args.Length == 1) {
                var threadId = (string) args[0];
                return (Lib.ICommand) new ThreadHardStopCommand(threadId);
            }
            else if (args.Length == 2) {
                var threadId = (string) args[0];
                var action = (Action) args[1];
                
                var thread = IoC.Resolve<ServerThread>("Threads.GetThreadById", threadId);

                var commandsList = new List<Lib.ICommand>();

                commandsList.Add(new UpdateBehaviourCommand(thread, action));
                commandsList.Add(new ThreadHardStopCommand(threadId));

                return (Lib.ICommand) new MacroCommand(commandsList);
            }
            else {
                throw new Exception();
            }
            
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Threads.SoftStopTheThread", (object[] args) => {
            var threadId = (string) args[0];
            return new ThreadSoftStopCommand(threadId);
        }).Execute();

        

        IoC.Resolve<ICommand>("IoC.Register", "Threads.Sender.MessageToCommand", (object[] args) => {
            var message = (Lib.ICommand) args[0];
            return message;
        }).Execute();
        return scope;
    }
    


    [Fact]
    public void Test1() {

        var scope = IoCdependency();

        Dictionary<string, object> dict = new Dictionary<string, object>() {};
        dict.Add("Position", new Vector(1, 1));
        dict.Add("Velocity", new Vector(0, 1));


        Mock<IUObject> UObject = new Mock<IUObject>();
        UObject.Setup(e => e.GetProperty(It.IsAny<string>())).Returns((string s) => {return dict[s];});
        UObject.Setup(e => e.SetProperty(It.Is<string>(x => x == "Position" || x == "Velocity"), It.IsAny<Vector>())).Callback((string s, object v) => dict[s] = v);

        
        Mock<SpaceBattle.Lib.ICommand> mockCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockCommand.Setup(e => e.Execute()).Callback(() => {
            var pos = (Vector) UObject.Object.GetProperty("Position");
            var vel = (Vector) UObject.Object.GetProperty("Velocity");
            UObject.Object.SetProperty("Position", pos + vel);
        });

        var thread = IoC.Resolve<ServerThread>("Threads.CreateAndStartThread", "1", ()=> {IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();}); //()=> {IoC.Resolve<ICommand>("Scope.Current.Set", scope).Execute();}
        System.Threading.Thread.Sleep(50);
        // Action a = () => {};

        // Assert.Equal(a, thread.strategy);

        var listcommands = new List<Lib.ICommand>();
        listcommands.Add(mockCommand.Object);
        //Assert.Equal(IoC.Resolve<ServerThread>("Threads.GetThreadById", "1"), thread);
        //listcommands.Add(new ThreadStopCommand(thread));
        listcommands.Add(IoC.Resolve<Lib.ICommand>("Threads.HardStopTheThread", "1", ()=> {IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();})); //
        IoC.Resolve<Lib.ICommand>("Threads.SendCommand", "1", listcommands).Execute();
        
        // BlockingCollection<SpaceBattle.Lib.ICommand> queue = new BlockingCollection<SpaceBattle.Lib.ICommand>(1000);
        // var receiver = new ReceiverAdapter(queue);
        // queue.Add(mockCommand.Object);
        // ServerThread thread = new ServerThread(receiver);
        // System.Console.WriteLine("Start");
        // thread.Start();


        System.Threading.Thread.Sleep(60);
        Assert.Equal(UObject.Object.GetProperty("Position"), new Vector(1, 2));


        //thread.Stop();

    }
 
}
