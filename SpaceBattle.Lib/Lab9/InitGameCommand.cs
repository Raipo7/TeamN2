using Hwdtech;
using System.Collections;
using System.Collections.Generic;

namespace SpaceBattle.Lib;

public class InitGameCommand : ICommand {
    
    private int gameObjectCount;
    public InitGameCommand(int gameObjectCount) {
        this.gameObjectCount = gameObjectCount;
    }

    public void Execute() {
        // IoC.Resolve("Game.CreateObject", gameObjectCount);
        var itemsId =  (IEnumerable) new CreateObjectStrategy().Execute(6); //можно через IoC || создаются пустые объекты в n кол-ве, и возвращаются их уникальные id в игре



        //IoC.Resolve<IStrategy>("Game.Init", "3 vs 3", itemsId); 
        {

            var queuePos = new Queue<int>();
            queuePos.Enqueue(1); queuePos.Enqueue(2); queuePos.Enqueue(4);
            queuePos.Enqueue(5); queuePos.Enqueue(6); queuePos.Enqueue(8);
            
            var queueFuel = new Queue<int>();
            queueFuel.Enqueue(100); queueFuel.Enqueue(100); queueFuel.Enqueue(100);
            queueFuel.Enqueue(100); queueFuel.Enqueue(100); queueFuel.Enqueue(100);

            //Queue or IEnumerable {1 2 4 5 6 8} IoC.
            //  4 3 2
            //  5 0 1
            //  6 7 8
            //new PosIterator({1 2 4 5 6 8})
            //new FuelIterator({100, 100, 100, 100, 100, 100})
            
            var posIterator = new GetPosIterator(queuePos);
            foreach (var itemId in itemsId) {
                posIterator.MoveNext();
                IoC.Resolve<ICommand>("Game.Items.SetProperty", "position", posIterator.Current).Execute();
                IoC.Resolve<ICommand>("Game.Items.SetProperty", "Fuel", posIterator.Current).Execute();
            }
        }


    }
}

