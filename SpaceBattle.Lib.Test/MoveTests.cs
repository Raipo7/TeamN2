using Xunit;
using Moq;
using System;
namespace SpaceBattle.Lib.Test;

public class MoveTest
{
    [Fact]
    public void MoveTestPositive()
    {
        Mock<IMovable> movableMock = new Mock<IMovable>();
        movableMock.SetupProperty<Vector>(m=>m.Position, new Vector(12, 5));
        movableMock.SetupGet<Vector>(m=>m.Velocity).Returns(new Vector(-7, 3));
        MoveCommand mcommand = new MoveCommand(movableMock.Object);
        mcommand.Execute();
        Assert.True(new Vector(5, 8) == movableMock.Object.Position);
    }
    [Fact]
    public void MoveTestGetPositionExeption()
    {
        Mock<IMovable> movableMock = new Mock<IMovable>();
        movableMock.SetupProperty<Vector>(m=>m.Position, It.IsAny<Vector>());
        movableMock.SetupGet<Vector>(m=>m.Position).Throws<ArgumentException>();
        movableMock.SetupGet<Vector>(m=>m.Velocity).Returns(It.IsAny<Vector>());
        MoveCommand mcommand = new MoveCommand(movableMock.Object);
        Assert.Throws<ArgumentException>(()=>mcommand.Execute());
    }
    [Fact]
    public void MoveTestSetPositionExeption()
    {
        Mock<IMovable> movableMock = new Mock<IMovable>();
        movableMock.SetupProperty(i => i.Position, new Vector(1, 2));
        movableMock.SetupSet<Vector>(m=>m.Position = It.IsAny<Vector>()).Throws<ArgumentException>();
        movableMock.SetupGet<Vector>(m=>m.Velocity).Returns(new Vector(2, 3));
        MoveCommand mcommand = new MoveCommand(movableMock.Object);
        Assert.Throws<ArgumentException>(()=>mcommand.Execute());
    }
    [Fact]
    public void MoveTestGetVelocityExeption()
    {
        Mock<IMovable> movableMock = new Mock<IMovable>();
        movableMock.SetupProperty<Vector>(m=>m.Position, It.IsAny<Vector>());
        movableMock.SetupGet<Vector>(m=>m.Velocity).Throws<ArgumentException>();
        MoveCommand mcommand = new MoveCommand(movableMock.Object);
        Assert.Throws<ArgumentException>(()=>mcommand.Execute());
    }
}