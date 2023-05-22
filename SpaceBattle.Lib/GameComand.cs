using Hwdtech;
using System.Diagnostics;

namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    private IReceiver receiver;
    private IDictionary<string, object> gameItems;

    private object scope;
    private int gameTick;

    private Stopwatch time;

    public GameCommand(object scope)
    {
        this.scope = scope;
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope);

        this.receiver = IoC.Resolve<IReceiver>("Game.GetReceiver");
        this.gameItems = IoC.Resolve<IDictionary<string, object>>("Game.GetItems");
        this.gameTick = IoC.Resolve<int>("Game.GetTick");
        time = new Stopwatch();
    }
    public void Execute()
    {
        time.Start();
        while (time.ElapsedMilliseconds <= gameTick)
        {
            if (!receiver.isEmpty())
            {
                var cmd = this.receiver.Receive();
                try
                {
                    cmd.Execute();
                }
                catch (Exception err)
                {
                    var exceptinHandlerStrategy = IoC.Resolve<IStrategy>("Exception.FindHandlerStrategy", cmd, err);
                    exceptinHandlerStrategy.Execute();
                }
            }
            else break;
        }
        time.Reset();
    }
}
