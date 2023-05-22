namespace SpaceBattle.Lib;
using System.Collections.Generic;
using Hwdtech;
using System.Diagnostics;
public class GameCommand : ICommand {

    private string gameId;
    private IReceiver receiver;
    private IDictionary<string, object> gameItems;

    private object scope;
    private int gameTick;

    private Stopwatch time;

    public GameCommand(object scope, IReceiver receiver, string gameId) {
        this.scope = scope;
        this.receiver = receiver;
        this.gameId = gameId;
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        this.gameItems = IoC.Resolve<IDictionary<string, object>>("Game.GetItems");
        this.gameTick = IoC.Resolve<int>("Game.GetTick");
        this.time = new Stopwatch();
    }

    public void Execute() {
        time.Start();
        while (time.ElapsedMilliseconds <= gameTick) {
            receiver.Receive();
        }
        time.Reset();
    }
}