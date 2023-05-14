namespace SpaceBattle.Lib.Test;
using Xunit;
using System.Collections.Generic;
using Moq;
using Hwdtech;
public class TestCollision
{

    public TestCollision()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "CollisionGetData", (object[] args) =>
        {
            return new List<List<int>>() { new List<int>() { -2, 1, 0, 1 } };
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "CollisionGetDeltas", (object[] args) =>
        {
            return new CollisionGetDeltasStrategy().Execute(args);
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "CollisionCreateTree", (object[] args) =>
        {
            return new CollisionCreateTreeStrategy().Execute(args);
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "CollisionTreeSolution", (object[] args) =>
        {
            return new CollisionTreeSolutionStrategy().Execute(args);
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "CollisionGetTree", (object[] args) =>
        {
            List<List<int>> treeData = IoC.Resolve<List<List<int>>>("CollisionGetData");
            return IoC.Resolve<Dictionary<int, object>>("CollisionCreateTree", treeData);
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "CollisionCheck", (object[] args) =>
        {
            return new CollisionCheckStrategy().Execute(args);
        }).Execute();
    }

    [Fact]
    public void CollisionCreateTreeTest()
    {
        List<List<int>> list = new() { new List<int>() { 2, 3 }, new List<int>() { 2, 4 } };
        Dictionary<int, object> b = (Dictionary<int, object>)new CollisionCreateTreeStrategy().Execute(list);
        Assert.Single(b);
        Assert.True(((Dictionary<int, object>)b[2]).ContainsKey(4));

    }

    [Fact]
    public void CollisionGetDeltasTest()
    {
        Mock<IUObject> UObject1 = new Mock<IUObject>();
        Mock<IUObject> UObject2 = new Mock<IUObject>();
        UObject1.Setup(x => x.GetProperty("Position")).Returns(new Vector(3, 1));
        UObject2.Setup(x => x.GetProperty("Position")).Returns(new Vector(1, 2));
        UObject1.Setup(x => x.GetProperty("Velocity")).Returns(new Vector(1, 1));
        UObject2.Setup(x => x.GetProperty("Velocity")).Returns(new Vector(1, 2));
        List<int> a = (List<int>)new CollisionGetDeltasStrategy().Execute(UObject1.Object, UObject2.Object);
        Assert.Equal(-2, a[0]);
        Assert.Equal(1, a[1]);
        Assert.Equal(0, a[2]);
        Assert.Equal(1, a[3]);
    }
    [Fact]
    public void CollisionCheckTestPositive()
    {

        Mock<IUObject> UObject1 = new Mock<IUObject>();
        Mock<IUObject> UObject2 = new Mock<IUObject>();
        UObject1.Setup(x => x.GetProperty("Position")).Returns(new Vector(3, 1));
        UObject2.Setup(x => x.GetProperty("Position")).Returns(new Vector(1, 2));
        UObject1.Setup(x => x.GetProperty("Velocity")).Returns(new Vector(1, 1));
        UObject2.Setup(x => x.GetProperty("Velocity")).Returns(new Vector(1, 2));

        Assert.Throws<System.Exception>(() => IoC.Resolve<SpaceBattle.Lib.ICommand>("CollisionCheck", UObject1.Object, UObject2.Object).Execute());

    }
    [Fact]
    public void CollisionCheckTestNegitive()
    {

        Mock<IUObject> UObject1 = new Mock<IUObject>();
        Mock<IUObject> UObject2 = new Mock<IUObject>();
        UObject1.Setup(x => x.GetProperty("Position")).Returns(new Vector(2, 1));
        UObject2.Setup(x => x.GetProperty("Position")).Returns(new Vector(1, 2));
        UObject1.Setup(x => x.GetProperty("Velocity")).Returns(new Vector(1, 1));
        UObject2.Setup(x => x.GetProperty("Velocity")).Returns(new Vector(1, 2));

        IoC.Resolve<SpaceBattle.Lib.ICommand>("CollisionCheck", UObject1.Object, UObject2.Object).Execute();
    }
}
