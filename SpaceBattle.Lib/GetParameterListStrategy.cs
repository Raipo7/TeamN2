namespace SpaceBattle.Lib;
using System.Reflection;

public class GetParameterListStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        MethodInfo method = (MethodInfo) args[0];
        ParameterInfo[] parameters = method.GetParameters();
        int key = (int) args[1];
        // if (key == 0 && parameters.Length > 0)
        // {
        //     string parameterList = string.Join(", ", parameters.Select(p => $"{p.ParameterType} {p.Name}"));
        //     return parameterList;
        // }
        // else if (key == 1 && parameters.Length > 0)
        // {
        //     string parameterList = ", " + string.Join(" ,", parameters.Select(p => $"{p.Name}"));
        //     return parameterList;  
        // }
        if (parameters.Length > 0)
        {
            if (key == 0)
            {
                string parameterList = string.Join(", ", parameters.Select(p => $"{p.ParameterType} {p.Name}"));
                return parameterList;
            }
            else
            {
                string parameterList = ", " + string.Join(" ,", parameters.Select(p => $"{p.Name}"));
                return parameterList;  
            }
        }
        return "";
    }
}
