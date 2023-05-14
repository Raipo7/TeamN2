namespace SpaceBattle.Lib.Test;
using Xunit;
using System.Collections.Generic;
using Hwdtech;
using Moq;

public class CollisionCheckFromFile
{

    public CollisionCheckFromFile()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        Mock<IStrategy> mockStrategy = new Mock<IStrategy>();
        Dictionary<int, object> dict = new(){
        {
            1, new Dictionary<int, object>(){
            {
                2, new Dictionary<int, object>(){
                {
                    4, new Dictionary<int, object>(){{4, new Dictionary<int, object>()}, {5, new Dictionary<int, object>()}}
                }}
            }}
        }};

        mockStrategy.Setup(x => x.Execute(It.IsAny<List<List<int>>>())).Returns(dict);

        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "CollisionGetData", (object[] args) =>
        {
            return new CollisionGetDataFromFileStrategy().Execute(args);
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "CollisionCreateTree", (object[] args) =>
        {
            return mockStrategy.Object.Execute(args);
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "CollisionGetTree", (object[] args) =>
        {
            List<List<int>> treeData = IoC.Resolve<List<List<int>>>("CollisionGetData", args);
            return IoC.Resolve<Dictionary<int, object>>("CollisionCreateTree", treeData);
        }).Execute();
    }

    [Fact]
    public void CreateCollisionDataFromFile()
    {

        List<List<int>> collisionData = (List<List<int>>)new CollisionGetDataFromFileStrategy().Execute("../../../TestCol.txt");
        Assert.Equal(4, collisionData[1][2]);
        Assert.Equal(5, collisionData[1][3]);
    }
    [Fact]
    public void Testing32()
    {
        Dictionary<int, object> dict = IoC.Resolve<Dictionary<int, object>>("CollisionGetTree", "../../../TestCol.txt");
        Assert.True(dict.ContainsKey(1));

        Dictionary<int, object> dict1 = (Dictionary<int, object>)dict[1];
        Assert.True(dict1.ContainsKey(2));

        Dictionary<int, object> dict2 = (Dictionary<int, object>)dict1[2];
        Assert.True(dict2.ContainsKey(4));

        Dictionary<int, object> dict3 = (Dictionary<int, object>)dict2[4];
        Assert.True(dict3.ContainsKey(4));
        Assert.True(dict3.ContainsKey(5));
    }
}
