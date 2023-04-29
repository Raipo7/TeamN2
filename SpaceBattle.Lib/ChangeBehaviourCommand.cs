namespace SpaceBattle.Lib;
using System.Collections.Concurrent;

public class ChangeBehaviourCommand : ICommand
{
    private ServerThreadStrategy serverThread;
    Action newBehaviour;
    public ChangeBehaviourCommand(ServerThreadStrategy thread, Action behaviour)
    {
        this.serverThread = thread;
        this.newBehaviour = behaviour;
    }
    public void Execute()
    {
        serverThread.ChangeBehaviour(newBehaviour);
    }
}
