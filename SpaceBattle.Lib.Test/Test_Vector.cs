namespace SpaceBattle.Lib.Test;
using Xunit;
using Moq;

public class TestVector
{
    [Fact]
    public void VectorConstructorEmptyException()
    {
        Assert.Throws<System.Exception>(() => new Vector());
    }
    [Fact]
    public void VectorToString()
    {
        Vector a = new Vector(1, 2);
        Assert.Equal("Vector(1, 2)", a.ToString());
    }
    [Fact]
    public void VectorEqual()
    {
        Assert.True(new Vector(3, 5) == new Vector(3, 5));
    }
    [Fact]
    public void VectorEqualException()
    {
        Assert.False(new Vector(3, 5, 6) == new Vector(2, 2));
    }
    [Fact]
    public void VectorNotEqual()
    {
        Assert.True(new Vector(3, 5) != new Vector(2, 2));
    }
    [Fact]
    public void VectorSum()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(2, 3);
        Assert.True(a + b == new Vector(3, 5));
    }
    [Fact]
    public void VectorSumSizeException()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(2, 3, 3);
        Assert.Throws<System.Exception>(() => a + b);
    }
    [Fact]
    public void VectorDiff()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(2, 3);
        Assert.True(a - b == new Vector(-1, -1));
    }
    [Fact]
    public void VectorDiffSizeException()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(2, 3, 3);
        Assert.Throws<System.Exception>(() => a - b);
    }
    [Fact]
    public void VectorEqualsCommandPositive()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(1, 2);
        Assert.True(a.Equals(b));
    }
    [Fact]
    public void VectorEqualsCommandNegative1()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(3, 2);
        Assert.False(a.Equals(b));
    }
    [Fact]
    public void VectorEqualsCommandNegative2()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(3, 2, 4);
        Assert.False(a.Equals(b));
    }
    [Fact]
    public void VectorEqualsCommandException()
    {
        Vector a = new Vector(1, 2);
        int b = 2;
        Assert.Throws<System.Exception>(() => a.Equals(b));
    }
    [Fact]
    public void VectorHashCodePositive()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(1, 2);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }
    [Fact]
    public void VectorHashCodeNegative()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(2, 1);
        Assert.False(a.GetHashCode() == b.GetHashCode());
    }

}
