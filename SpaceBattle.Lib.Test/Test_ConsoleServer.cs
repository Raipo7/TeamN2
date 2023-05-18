namespace SpaceBattle.Lib.Test;
using Xunit;
using Moq;
using System.Collections.Generic;
using Hwdtech;

public class Test_ServerStart{
    public object IoCdependency() {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();
        return scope;
    }
    [Fact]
    public void Execute_CreatesAndStartsThreads()
    {
        int numOfThread = 5;

        var startServerCommand = new StartServerCommand(numOfThread);
        int threadCreateCallCount = 0;
        int threadsStartCallCount = 0;

        IoC.Resolve<int>("IoC.Register", "Thread.Create", () => {
            threadCreateCallCount++;
            return 1;
        });
        IoC.Resolve<ICommand>("IoC.Register", "Thread.Start", (object[] args) => {
            threadsStartCallCount++;
            return Mock.Of<ICommand>();
        }).Execute();
        
        startServerCommand.Execute();

        Assert.Equal(numOfThread, threadCreateCallCount);
        Assert.Equal(numOfThread, threadsStartCallCount);
    }
}
