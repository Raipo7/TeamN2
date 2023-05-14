namespace SpaceBattle.Lib.Test;
using Xunit;
using Moq;

public class TestRotateCommand
{
    [Fact]
    public void RotateCommandTestPositive()
    {
        Mock<IRotateble> rotate_mock = new Mock<IRotateble>();
        rotate_mock.SetupProperty(i => i.Angle, new Angle(1, 4));
        rotate_mock.SetupGet<Angle>(i => i.AngleVelocity).Returns(new Angle(1, 2));
        new RotateCommand(rotate_mock.Object).Execute();
        Assert.Equal(new Angle(3, 4), rotate_mock.Object.Angle);
    }
    [Fact]
    public void RotateCommandTestGetAngleException()
    {
        Mock<IRotateble> rotate_mock = new Mock<IRotateble>();
        rotate_mock.SetupProperty(i => i.Angle, new Angle(1, 4));
        rotate_mock.SetupGet<Angle>(i => i.Angle).Throws<System.Exception>();
        rotate_mock.SetupGet<Angle>(i => i.AngleVelocity).Returns(new Angle(1, 2));
        Assert.Throws<System.Exception>(() => new RotateCommand(rotate_mock.Object).Execute());
    }
    [Fact]
    public void RotateCommandTestGetAngularVelocityException()
    {
        Mock<IRotateble> rotate_mock = new Mock<IRotateble>();
        rotate_mock.SetupProperty(i => i.Angle, new Angle(1, 4));
        rotate_mock.SetupGet<Angle>(i => i.AngleVelocity).Throws<System.Exception>();
        Assert.Throws<System.Exception>(() => new RotateCommand(rotate_mock.Object).Execute());
    }
    [Fact]
    public void RotateCommandTestSetAngleException()
    {
        Mock<IRotateble> rotate_mock = new Mock<IRotateble>();
        rotate_mock.SetupProperty(i => i.Angle, new Angle(1, 4));
        rotate_mock.SetupSet<Angle>(i => i.Angle = It.IsAny<Angle>()).Throws<System.Exception>();
        rotate_mock.SetupGet<Angle>(i => i.AngleVelocity).Returns(new Angle(1, 2));
        Assert.Throws<System.Exception>(() => new RotateCommand(rotate_mock.Object).Execute());
    }
}
