namespace SpaceBattle.Lib;

public class Vector
{
    public int[] cords {get; private set;}

    public Vector(params int[] nums)
    {
        if (nums.Length == 0)
        {
            throw new Exception("Vector cannot be empty");
        }
        this.cords = nums;
    }
    public override string ToString()
    {
        string s = "Vector(";
        for (int i = 0; i < cords.Length; i++)
        {
            if (i != cords.Length - 1) s += cords[i] + ", ";
            else s += cords[i];
        }
        return s + ")";
    }
    public static Vector operator +(Vector v1, Vector v2)
    {
        if (v1.cords.Length == v2.cords.Length)
        {
            int[] sum = new int[v1.cords.Length];
            for (int i = 0; i < v1.cords.Length; i++)
            {
                sum[i] = v1.cords[i] + v2.cords[i];
            }
            return new Vector(sum);
        }
        else
        {
            throw new Exception("Vector lengths are not equal");
        }
    }
    public static Vector operator -(Vector v1, Vector v2)
    {
        if (v1.cords.Length == v2.cords.Length)
        {
            int[] difference = new int[v1.cords.Length];
            for (int i = 0; i < v1.cords.Length; i++)
            {
                difference[i] = v1.cords[i] - v2.cords[i];
            }
            return new Vector(difference);
        }
        else
        {
            throw new Exception("Vector lengths are not equal");
        }
    }
    public static bool operator ==(Vector v1, Vector v2)
    {
        if (v1.cords.Length != v2.cords.Length)
        {
            return false;
        }
        for (int i = 0; i < v1.cords.Length; i++)
        {
            if (v1.cords[i] != v2.cords[i])
            {
                return false;
            }
        }
        return true;
    }
    public static bool operator !=(Vector v1, Vector v2)
    {
        return !(v1 == v2);
    }
    public override bool Equals(object? obj)
    {
        if (obj is not Vector)
        {
            throw new Exception("Trying to compare with a no Vector");
        }
        Vector is_Vector = (Vector)obj;
        if (is_Vector.cords.Length != cords.Length)
        {
            return false;
        }
        for (int i = 0; i < cords.Length; i++)
        {
            if (is_Vector.cords[i] != cords[i])
            {
                return false;
            }
        }
        return true;

    }
    public override int GetHashCode()
    {
        int this_HashCode = 0;
        foreach (int i in cords)
        {
            this_HashCode = HashCode.Combine(this_HashCode, i.GetHashCode());
        }
        return this_HashCode;
    }
}
