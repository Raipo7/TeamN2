namespace SpaceBattle.Lib.Test;
using Xunit;
using Moq;

public class TestAngle
{
    [Fact]
    public void AngleConstructorZeroDenominator()
    {
        Assert.Throws<System.Exception>(() => new Angle(1, 0));
    }
    [Fact]
    public void AngleToString()
    {
        Angle a = new Angle(1, 2);
        Angle b = new Angle(-3, 2);
        Angle c = new Angle(3, 6);
        Angle d = new Angle(-9, 6);
        Assert.Equal(a.ToString(), "AnglePi(1/2)");
        Assert.Equal(b.ToString(), "AnglePi(1/2)");
        Assert.Equal(c.ToString(), "AnglePi(1/2)");
        Assert.Equal(d.ToString(), "AnglePi(1/2)");
    }
    [Fact]
    public void AngleEqualPositive()
    {
        Assert.True(new Angle(1, 2) == new Angle(-3, 2));
        Assert.True(new Angle(3, 6) == new Angle(-3, 2));
        Assert.True(new Angle(-9, 6) == new Angle(-3, 2));
    }
    [Fact]
    public void AngleNotEqual()
    {
        Assert.True(new Angle(1, 2) != new Angle(-4, 2));
        Assert.True(new Angle(3, 6) != new Angle(-3, -6));
        Assert.True(new Angle(-9, 6) != new Angle(2, 2));
    }
    [Fact]
    public void AngleSum()
    {
        Assert.Equal(new Angle(4, 1) + new Angle(-1, 1), new Angle(10, 10));
        Assert.Equal(new Angle(-1, 4) + new Angle(-1, 4), new Angle(3, 2));
        Assert.Equal(new Angle(4, 3) + new Angle(5, 4), new Angle(7, 12));
    }
    [Fact]
    public void AngleDiff()
    {
        Assert.Equal(new Angle(1, 2) - new Angle(2, 4), new Angle(0, 10));
        Assert.Equal(new Angle(-1, 4) - new Angle(1, 4), new Angle(3, 2));
        Assert.Equal(new Angle(1, 4) - new Angle(1, 3), new Angle(23, 12));
    }
    [Fact]
    public void AngleEqualsCommandPositive()
    {
        Angle a = new Angle(1, 2);
        Angle b = new Angle(-3, 2);
        Assert.True(a.Equals(b));
    }
    [Fact]
    public void AngleEqualsCommandNegative()
    {
        Angle a = new Angle(1, 2);
        Angle b = new Angle(3, 2);
        Angle c = new Angle(1, 3);
        Assert.False(a.Equals(b));
        Assert.False(a.Equals(c));
    }
    [Fact]
    public void AngleEqualsCommandException()
    {
        Angle a = new Angle(1, 2);
        int b = 2;
        Assert.Throws<System.Exception>(() => a.Equals(b));
    }
    [Fact]
    public void AngleGetHashCode()
    {
        Angle a = new Angle(1, 2);
        Angle b = new Angle(-3, 2);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }
}
