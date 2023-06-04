namespace SpaceBattle.Lib;
using System;
using Hwdtech;
using System.Reflection;
using System.Text;

public class AdapterBuilderStrategy : IStrategy
{
    public object Execute(params object[] args)
    {   
        Type interfaceType = (Type) args[0];
        Type adapterType = (Type) args[1];
        StringBuilder codeBuilder = new StringBuilder();

        codeBuilder.AppendLine($"public class {adapterType.Name} : {interfaceType.FullName}");
        codeBuilder.AppendLine("{");
        codeBuilder.AppendLine("\tprivate object target;");

        codeBuilder.AppendLine($"\tpublic {adapterType.Name}(object target)");
        codeBuilder.AppendLine("\t{");
        codeBuilder.AppendLine("\t\tthis.target = target;");
        codeBuilder.AppendLine("\t}");

        MethodInfo[] methods = interfaceType.GetMethods();
        foreach (MethodInfo method in methods)
        {
            string parameterList = IoC.Resolve<string>("GetParameterList", method);
            codeBuilder.AppendLine();
            codeBuilder.AppendLine($"\tpublic void {method.Name}({parameterList})");
            codeBuilder.AppendLine("\t{");
            codeBuilder.AppendLine($"\t\treturn IoC.Resolve<{method.ReturnType}>(\"SpaceShip.{method.Name}\", {parameterList}).Execute();");
            codeBuilder.AppendLine("\t}");
        }

        codeBuilder.AppendLine("}");

        return codeBuilder.ToString();
    }
}
