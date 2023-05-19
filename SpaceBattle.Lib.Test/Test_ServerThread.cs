namespace SpaceBattle.Lib.Test;
using Xunit;
using Moq;
using Hwdtech;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

public class Test_ServerThread
{
    public string testString = "";

    public Test_ServerThread()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        Dictionary<string, ServerThread> dictThreads = new Dictionary<string, ServerThread>();
        Dictionary<string, ISender> dictSenders = new Dictionary<string, ISender>();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.GETA", (object[] args) =>
        {
            return dictThreads;

        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.GetThreadById", (object[] args) =>
        {
            var threadId = (string)args[0];
            return dictThreads[threadId];

        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.GetIdByThread", (object[] args) =>
        {
            var thread = (ServerThread)args[0];

            foreach (var (key, value) in dictThreads)
            {
                if (value == thread)
                {
                    return key;
                }
            }
            throw new System.Exception();

        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.GetSenderById", (object[] args) =>
        {
            var threadId = (string)args[0];
            return dictSenders[threadId];

        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.CreateAndStartThread", (object[] args) =>
        {
            if (args.Length == 1)
            {
                var threadId = (string)args[0];

                BlockingCollection<Lib.ICommand> queue = new BlockingCollection<Lib.ICommand>();

                ISender sender = new SenderAdapter(queue);
                IReceiver receiver = new ReceiverAdapter(queue);
                ServerThread thread = new ServerThread(receiver);

                thread.Start();

                dictSenders.Add(threadId, sender);
                dictThreads.Add(threadId, thread);
                return thread;
            }
            else if (args.Length == 2)
            {
                var threadId = (string)args[0];
                var action = (Action)args[1];

                BlockingCollection<Lib.ICommand> queue = new BlockingCollection<Lib.ICommand>();

                ISender sender = new SenderAdapter(queue);
                IReceiver receiver = new ReceiverAdapter(queue);
                ServerThread thread = new ServerThread(receiver);

                queue.Add(new ActionCommand(action));

                thread.Start();

                dictSenders.Add(threadId, sender);
                dictThreads.Add(threadId, thread);
                return thread;
            }
            else
            {
                throw new Exception();
            }
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.SendCommand", (object[] args) =>
        {
            if (args.Length == 2)
            {
                var threadId = (string)args[0];
                var sender = IoC.Resolve<ISender>("Thread.GetSenderById", threadId);
                try
                {
                    var command = (Lib.ICommand)args[1];
                    return (Lib.ICommand)new SendCommand(sender, command);
                }
                catch
                {
                    var commandsList = (IEnumerable<Lib.ICommand>)args[1];
                    var macroCommandsList = new List<Lib.ICommand>();
                    foreach (var command in commandsList)
                    {
                        macroCommandsList.Add(new SendCommand(sender, command));
                    }
                    return (Lib.ICommand)new MacroCommand(macroCommandsList);
                }
            }
            else
            {
                throw new Exception();
            }
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.HardStopTheThread", (object[] args) =>
        {
            if (args.Length == 1)
            {
                var threadId = (string)args[0];
                var thread = IoC.Resolve<ServerThread>("Thread.GetThreadById", threadId);
                return (Lib.ICommand)new ThreadHardStopCommand(thread);
            }
            else if (args.Length == 2)
            {
                var threadId = (string)args[0];
                var action = (Action)args[1];
                var thread = IoC.Resolve<ServerThread>("Thread.GetThreadById", threadId);
                return new ThreadHardStopCommand(thread, action);
            }
            else
            {
                throw new Exception();
            }
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.SoftStopTheThread", (object[] args) =>
        {
            if (args.Length == 1)
            {
                var threadId = (string)args[0];
                var thread = IoC.Resolve<ServerThread>("Thread.GetThreadById", threadId);
                return new ThreadSoftStopCommand(thread);
            }
            else if (args.Length == 2)
            {
                var threadId = (string)args[0];
                var action = (Action)args[1];
                var thread = IoC.Resolve<ServerThread>("Thread.GetThreadById", threadId);
                return new ThreadSoftStopCommand(thread, action);
            }
            else
            {
                throw new Exception();
            }
        }).Execute();

        Dictionary<int, Dictionary<int, IStrategy>> exceptionDict = new Dictionary<int, Dictionary<int, IStrategy>>();
        Dictionary<int, IStrategy> exceptionNotFoundCommand = new Dictionary<int, IStrategy>();
        Mock<IStrategy> exceptionNotFoundExcepetion = new Mock<IStrategy>();

        exceptionNotFoundExcepetion.Setup(x => x.Execute()).Callback(() => testString = "Command Exception");

        IoC.Resolve<ICommand>("IoC.Register", "Exception.FindHandlerStrategy", (object[] args) =>
        {
            var cmd = (Lib.ICommand)args[0];
            var err = (Exception)args[1];
            return new ExceptionFindHandlerStrategy().Execute(cmd, err);
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Get.Tree", (object[] args) =>
        {
            return exceptionDict;
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Get.NotFoundCommandSubTree", (object[] args) =>
        {
            return exceptionNotFoundCommand;
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Get.NotFoundExcepetionHandler", (object[] args) =>
        {
            return exceptionNotFoundExcepetion.Object;
        }).Execute();
        this.globalScope = scope;
    }

    object globalScope;

    [Fact]
    public void Test_CreateAndStartThread_with_HardStopTheThread_AndSoftStopTheThread()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>() { };
        dict.Add("Position", new Vector(0, 0));
        dict.Add("Velocity", new Vector(1, 1));

        Mock<IUObject> UObject = new Mock<IUObject>();
        UObject.Setup(e => e.GetProperty(It.IsAny<string>())).Returns((string s) => { return dict[s]; });
        UObject.Setup(e => e.SetProperty(It.Is<string>(x => x == "Position" || x == "Velocity"), It.IsAny<Vector>())).Callback((string s, object v) => dict[s] = v);

        Mock<SpaceBattle.Lib.ICommand> mockCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockCommand.Setup(e => e.Execute()).Callback(() =>
        {
            var pos = (Vector)UObject.Object.GetProperty("Position");
            var vel = (Vector)UObject.Object.GetProperty("Velocity");
            UObject.Object.SetProperty("Position", pos + vel);
        });

        var threadForHardStop = IoC.Resolve<ServerThread>("Thread.CreateAndStartThread", "1", () => IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", globalScope)).Execute());

        var listcommands = new List<Lib.ICommand>();
        listcommands.Add(mockCommand.Object);
        listcommands.Add(mockCommand.Object);
        listcommands.Add(IoC.Resolve<Lib.ICommand>("Thread.HardStopTheThread", "1"));
        IoC.Resolve<Lib.ICommand>("Thread.SendCommand", "1", listcommands).Execute();
        System.Threading.Thread.Sleep(100);

        Assert.Equal(UObject.Object.GetProperty("Position"), new Vector(2, 2));

        var threadForSoftStop = IoC.Resolve<ServerThread>("Thread.CreateAndStartThread", "2", () => IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", globalScope)).Execute());
        
        listcommands.Clear();
        listcommands.Add(IoC.Resolve<Lib.ICommand>("Thread.SoftStopTheThread", "2"));
        listcommands.Add(mockCommand.Object);
        listcommands.Add(mockCommand.Object);
        listcommands.Add(mockCommand.Object);
        listcommands.Add(mockCommand.Object);
        IoC.Resolve<Lib.ICommand>("Thread.SendCommand", "2", listcommands).Execute();
        System.Threading.Thread.Sleep(100);

        Assert.Equal(UObject.Object.GetProperty("Position"), new Vector(6, 6));
    }
    [Fact]
    public void Test_Thread_ExceptionHandler()
    {
        Mock<Lib.ICommand> noCommand = new Mock<Lib.ICommand>();
        noCommand.Setup(x => x.Execute()).Throws(new Exception());

        var thread = IoC.Resolve<ServerThread>("Thread.CreateAndStartThread", "3", () => IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", globalScope)).Execute());;
        
        var listcommands = new List<Lib.ICommand>();
        listcommands.Add(noCommand.Object);
        listcommands.Add(IoC.Resolve<Lib.ICommand>("Thread.HardStopTheThread", "3"));
        IoC.Resolve<Lib.ICommand>("Thread.SendCommand", "3", listcommands).Execute();
        System.Threading.Thread.Sleep(100);

        Assert.Equal("Command Exception", testString);
    }
    [Fact]
    public void Test_ThreadStopCommandException()
    {
        var thread1 = IoC.Resolve<ServerThread>("Thread.CreateAndStartThread", "4", () => IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", globalScope)).Execute());
        var thread2 = IoC.Resolve<ServerThread>("Thread.CreateAndStartThread", "5", () => IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", globalScope)).Execute());

        var listcommands = new List<Lib.ICommand>();
        listcommands.Add(new ThreadHardStopCommand(thread1));
        listcommands.Add(new ThreadHardStopCommand(thread2));
        
        IoC.Resolve<Lib.ICommand>("Thread.SendCommand", "4", listcommands).Execute();
        IoC.Resolve<Lib.ICommand>("Thread.SendCommand", "5", listcommands).Execute();
        System.Threading.Thread.Sleep(100);
    }
    [Fact]
    public void Test_Thread_HardAndSoft_StopCommandWithAction()
    {
        var testInt = 0;
        Action action = () =>
        {
            testInt += 1;
        };

        var thread1 = IoC.Resolve<ServerThread>("Thread.CreateAndStartThread", "6", () => IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", globalScope)).Execute());
        var thread2 = IoC.Resolve<ServerThread>("Thread.CreateAndStartThread", "7", () => IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", globalScope)).Execute());

        IoC.Resolve<Lib.ICommand>("Thread.SendCommand", "6", IoC.Resolve<Lib.ICommand>("Thread.HardStopTheThread", "6", action)).Execute();
        IoC.Resolve<Lib.ICommand>("Thread.SendCommand", "7", IoC.Resolve<Lib.ICommand>("Thread.SoftStopTheThread", "7", action)).Execute();
        System.Threading.Thread.Sleep(100);

        Assert.Equal(2, testInt);
    }
}
