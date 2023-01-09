namespace SpaceBattle.Lib;
using System.IO;
using System.Collections.Generic;

public class CollisionGetDataFromFileStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string fileName = (string)args[0];
        string[] fileLines = File.ReadAllLines(fileName);
        List<List<int>> collisionData = new();
        for (int i = 0; i < fileLines.Length; i++)
        {
            collisionData.Add(new List<int>());
            foreach (string num in fileLines[i].Split(" "))
            {
                collisionData[i].Add(System.Convert.ToInt32(num));
            }
        }
        return collisionData;
    }
}
