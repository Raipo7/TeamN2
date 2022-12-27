namespace SpaceBattle.Lib;

public class Angle
{
    public int numerator, denominator;
    private static int NOD(int a, int b)
    {
        if (b == 0)
        {
            return Math.Abs(a);
        }
        else
        {
            return NOD(b, a % b);
        }
    }
    public Angle(int numerator, int denominator)
    {
        if (denominator == 0)
        {
            throw new Exception("Denominator cannot be equal to 0");
        }
        numerator %= 2 * denominator;
        if (numerator < 0)
        {
            numerator += 2 * denominator;
        }
        int NOD_Angle = NOD(numerator, denominator);
        this.numerator = numerator / NOD_Angle;
        this.denominator = denominator / NOD_Angle;
    }
    public override string ToString()
    {
        return "AnglePi(" + numerator + "/" + denominator + ")";

    }
    public static Angle operator +(Angle a1, Angle a2)
    {
        Angle new_Angle = new Angle((a1.numerator * a2.denominator + a2.numerator * a1.denominator) % (2 * a1.denominator * a2.denominator), a1.denominator * a2.denominator);
        int NOD_Angle = NOD(new_Angle.numerator, new_Angle.denominator);
        new_Angle.numerator /= NOD_Angle;
        new_Angle.denominator /= NOD_Angle;
        return new_Angle;
    }
    public static Angle operator -(Angle a1, Angle a2)
    {
        Angle new_Angle = new Angle((a1.numerator * a2.denominator - a2.numerator * a1.denominator) % (2 * a1.denominator * a2.denominator), a1.denominator * a2.denominator);
        int NOD_Angle = NOD(new_Angle.numerator, new_Angle.denominator);
        new_Angle.numerator /= NOD_Angle;
        new_Angle.denominator /= NOD_Angle;
        return new_Angle;
    }
    public static bool operator ==(Angle a1, Angle a2)
    {
        return a1.numerator == a2.numerator && a1.denominator == a2.denominator;
    }
    public static bool operator !=(Angle a1, Angle a2)
    {
        return !(a1 == a2);
    }
    public override bool Equals(object? obj)
    {
        if (obj is not Angle)
        {
            throw new Exception("Trying to compare with a no Angle");
        }
        Angle is_angle = (Angle)obj;
        return is_angle.numerator == numerator && is_angle.denominator == denominator;

    }
    public override int GetHashCode()
    {
        return HashCode.Combine(numerator, denominator);
    }
}
