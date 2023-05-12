namespace SpaceBattle.Lib;

using System.Collections.Concurrent;

using Hwdtech;

using System.Collections.Generic;

public interface IReceiver {
    ICommand Receive();
    
    bool isEmpty();
}

public class ServerThread {
    bool stop = false;
    Thread thread;
    IReceiver queue;

    Action strategy;

    public ServerThread(IReceiver receiver) { 
        this.queue = receiver;

        this.strategy = () => {
            HandleCommand();
        };

        thread = new Thread(() => {
            while(!stop) {
                strategy();
            }
        });
    }
    public void Start() {
        thread.Start();
    }

    public void Stop() {
        stop = true;
    }

    public bool ThreadIsEmpty() {
        return queue.isEmpty();
    }

    public bool ThreadEqual(Thread secondThread) {
        return this.thread == secondThread;
    }
    internal void UpdateBehaviour(Action newBehaviour) {
        strategy = newBehaviour;
    }

    internal void HandleCommand() {
        var cmd = queue.Receive();
        try {
            cmd.Execute();
        }
        catch(Exception err) {
            var exceptinHandlerStrategy = (IStrategy) new ExceptionFindHandlerStrategy().Execute(cmd, err);
            exceptinHandlerStrategy.Execute();   
        }
            
        
    }
}

public class ActionCommand : ICommand {
    Action action;

    public ActionCommand(Action action) {
        this.action = action;
    }
    public void Execute() {
        this.action();
    }
}
public class ThreadStopCommand : ICommand {
    
    ServerThread thread;

    public ThreadStopCommand(ServerThread thread) {
        this.thread = thread;
    }

    public void Execute(){
        if (thread.ThreadEqual(Thread.CurrentThread)) {
            thread.Stop();
        }
        else {
            throw new Exception();
        }
    }
}

public class ThreadHardStopCommand : ICommand {
    
    string threadId;
    Action action = () => {};
    public ThreadHardStopCommand(string threadId) {
        this.threadId = threadId;
    }
    public ThreadHardStopCommand(string threadId, Action action) {
        this.threadId = threadId;
        this.action = action;
    }
    public void Execute() {
        var thread = IoC.Resolve<ServerThread>("Thread.GetThreadById", threadId);
        new ThreadStopCommand(thread).Execute();
        action();
    }
}

public class ThreadSoftStopCommand : ICommand {
    
    string threadId;

    Action action = () => {};
    public ThreadSoftStopCommand(string threadId) {
        this.threadId = threadId;
    }
    public ThreadSoftStopCommand(string threadId, Action action) {
        this.threadId = threadId;
        this.action = action;
    }
    public void Execute() {

        var thread = IoC.Resolve<ServerThread>("Thread.GetThreadById", threadId);
        new UpdateBehaviourCommand(thread, () => {
            
            if(!thread.ThreadIsEmpty()) {
                thread.HandleCommand();
            }
            else {
                IoC.Resolve<ICommand>("Thread.SendCommand", threadId, IoC.Resolve<ICommand>("Thread.HardStopTheThread", threadId)).Execute();
            }
        }).Execute();
        action();
    }
}


public class ReceiverAdapter : IReceiver {
    BlockingCollection<ICommand> queue;
    public ReceiverAdapter(BlockingCollection<ICommand> queue) {
        this.queue = queue;
    }
    public ICommand Receive() {
        return queue.Take();
    }
    public bool isEmpty() {
        return queue.Count == 0;
    }

}

public class UpdateBehaviourCommand : ICommand {
    ServerThread thread;
    Action newBehaviour;

    public UpdateBehaviourCommand(ServerThread thread, Action newBehaviour) {
        this.newBehaviour = newBehaviour;
        this.thread = thread;
    }

    public void Execute() {
        thread.UpdateBehaviour(newBehaviour);
    }
}


public interface ISender {
    void Send(object message);
}

public class SenderAdapter : ISender {
    public BlockingCollection<ICommand> queue;

    public SenderAdapter(BlockingCollection<ICommand> queue) {
        this.queue = queue;
    }

    public void Send(object message) {
        queue.Add(IoC.Resolve<ICommand>("Thread.Sender.MessageToCommand", message));
    }
}

public class SendCommand : ICommand {

    ISender sender;

    IEnumerable<ICommand> commandsList;

    public SendCommand(string threadId, IEnumerable<ICommand> commandsList) {
        this.sender = IoC.Resolve<ISender>("Thread.GetSenderById", threadId);

        this.commandsList = commandsList;
    }

    public void Execute() {
        foreach(var command in commandsList) {
            sender.Send(command);
        }
    }
}
