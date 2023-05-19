namespace SpaceBattle.Lib;

public class UpdateBehaviourCommand : ICommand
{
    ServerThread thread;
    Action newBehaviour;

    public UpdateBehaviourCommand(ServerThread thread, Action newBehaviour)
    {
        this.newBehaviour = newBehaviour;
        this.thread = thread;
    }

    public void Execute()
    {
        thread.UpdateBehaviour(newBehaviour);
    }
}
