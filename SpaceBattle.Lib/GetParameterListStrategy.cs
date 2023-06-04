namespace SpaceBattle.Lib;
using System.Reflection;

public class GetParameterListStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        MethodInfo method = (MethodInfo) args[0];
        ParameterInfo[] parameters = method.GetParameters();
        string parameterList = string.Join(", ", parameters.Select(p => $"{p.ParameterType} {p.Name}"));
        return parameterList;
    }
}
