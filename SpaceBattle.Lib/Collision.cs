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

        foreach (int delta in deltas)
        {
            if (step.ContainsKey(delta))
            {
                step = (Dictionary<int, object>)step[delta];
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}

public class CollisionCheckCommand : ICommand
{
    private IUObject UObject1;
    private IUObject UObject2;
    public CollisionCheckCommand(IUObject UObject1, IUObject UObject2)
    {
        this.UObject1 = UObject1;
        this.UObject2 = UObject2;
    }
    public void Execute()
    {
        List<int> deltas = IoC.Resolve<List<int>>("CollisionGetDeltas", UObject1, UObject2);
        Dictionary<int, object> tree = IoC.Resolve<Dictionary<int, object>>("CollisionGetTree");
        if (IoC.Resolve<bool>("CollisionTreeSolution", tree, deltas))
        {
            throw new Exception();
        }
    }
}

public class CollisionCheckStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        IUObject UObject1 = (IUObject)args[0];
        IUObject UObject2 = (IUObject)args[1];
        return new CollisionCheckCommand(UObject1, UObject2);
    }
}
