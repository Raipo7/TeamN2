namespace SpaceBattle.Lib.Test;
using Xunit;
using Moq;

public class TestMoveCommand
{
    [Fact]
    public void MoveCommandTestPositive()
    {
        Mock<IMovable> move_mock = new Mock<IMovable>();
        move_mock.SetupProperty(i => i.Position, new Vector(12, 5));
        move_mock.SetupGet<Vector>(i => i.Velocity).Returns(new Vector(-7, 3));
        new MoveCommand(move_mock.Object).Execute();
        Assert.Equal(new Vector(5, 8), move_mock.Object.Position);
    }
    [Fact]
    public void MoveCommandTestGetPositionExeption()
    {
        Mock<IMovable> move_mock = new Mock<IMovable>();
        move_mock.SetupProperty(i => i.Position, new Vector(12, 5));
        move_mock.SetupGet<Vector>(i => i.Position).Throws<System.Exception>();
        move_mock.SetupGet<Vector>(i => i.Velocity).Returns(new Vector(-7, 3));
        Assert.Throws<System.Exception>(() => new MoveCommand(move_mock.Object).Execute());
    }
    [Fact]
    public void MoveCommandTestGetVelocityException()
    {
        Mock<IMovable> move_mock = new Mock<IMovable>();
        move_mock.SetupProperty(i => i.Position, new Vector(12, 5));
        move_mock.SetupGet<Vector>(i => i.Velocity).Throws<System.Exception>();
        Assert.Throws<System.Exception>(() => new MoveCommand(move_mock.Object).Execute());
    }
    [Fact]
    public void MoveCommandTestSetPositionExeption()
    {
        Mock<IMovable> move_mock = new Mock<IMovable>();
        move_mock.SetupProperty(i => i.Position, new Vector(12, 5));
        move_mock.SetupSet<Vector>(i => i.Position = It.IsAny<Vector>()).Throws<System.Exception>();
        move_mock.SetupGet<Vector>(i => i.Velocity).Returns(new Vector(-7, 3));
        Assert.Throws<System.Exception>(() => new MoveCommand(move_mock.Object).Execute());
    }
}
