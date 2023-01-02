namespace SpaceBattle.Lib;

using Hwdtech;

public class CollisionCreateTreeStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        List<List<int>> collisionVectors = (List<List<int>>)args[0];
        Dictionary<int, object> tree = new();
        collisionVectors.ForEach(x =>
        {
            var step = tree;
            x.ForEach(y =>
            {
                step.TryAdd(y, new Dictionary<int, object>());
                step = (Dictionary<int, object>)step[y];
            });
        });
        return tree;
    }
}
public class CollisionGetDeltasStrategy : IStrategy
{

    public object Execute(params object[] args)
    {
        IUObject UObject1 = (IUObject)args[0];
        IUObject UObject2 = (IUObject)args[1];
        List<int> deltas = new();

        Vector position1 = (Vector)UObject1.GetProperty("Position");
        Vector position2 = (Vector)UObject2.GetProperty("Position");
        for (int i = 0; i < position1.cords.Count(); i++)
        {
            deltas.Add(position2.cords[i] - position1.cords[i]);
        }
        Vector velocity1 = (Vector)UObject1.GetProperty("Velocity");
        Vector velocity2 = (Vector)UObject2.GetProperty("Velocity");
        for (int i = 0; i < velocity1.cords.Count(); i++)
        {
            deltas.Add(velocity2.cords[i] - velocity1.cords[i]);
        }
        return deltas;
    }
}
public class CollisionTreeSolutionStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        Dictionary<int, object> tree = (Dictionary<int, object>)args[0];
        List<int> deltas = (List<int>)args[1];

        Dictionary<int, object> step = tree;

        for (int i = 0; i < deltas.Count() - 1; i++)
        {
            if (step.ContainsKey(deltas[i]))
            {
                step = (Dictionary<int, object>)step[deltas[i]];
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}

public class CollisionCheckStrategy : IStrategy
{

    public object Execute(params object[] args)
    {
        IUObject UObject1 = (IUObject)args[0];
        IUObject UObject2 = (IUObject)args[1];

        List<List<int>> treeData = IoC.Resolve<List<List<int>>>("CollisionGetData"); //получил строки
        List<int> deltas = IoC.Resolve<List<int>>("CollisionGetDeltas", UObject1, UObject2); //получил дельты
        Dictionary<int, object> tree = IoC.Resolve<Dictionary<int, object>>("CollisionCreateTree", treeData);
        return IoC.Resolve<bool>("CollisionTreeSolution", tree, deltas);
    }

}
