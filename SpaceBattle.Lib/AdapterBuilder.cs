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
        StringBuilder codeBuilder = new StringBuilder();
        codeBuilder.AppendLine("namespace SpaceBattle.Lib;");
        codeBuilder.AppendLine("using Hwdtech;");
        codeBuilder.AppendLine();
        codeBuilder.AppendLine($"public class {interfaceType.Name}Adapter : {interfaceType.Name}");
        codeBuilder.AppendLine("{");
        codeBuilder.AppendLine("\tprivate object target;");

        codeBuilder.AppendLine($"\tpublic {interfaceType.Name}Adapter(object target)");
        codeBuilder.AppendLine("\t{");
        codeBuilder.AppendLine("\t\tthis.target = target;");
        codeBuilder.AppendLine("\t}");

        MethodInfo[] methods = interfaceType.GetMethods();
        foreach (MethodInfo method in methods)
        {
            string parameterList1 = IoC.Resolve<string>("Interface.GetParameterList", method, 0);
            string parameterList2 = IoC.Resolve<string>("Interface.GetParameterList", method, 1);
            codeBuilder.AppendLine($"\tpublic {method.ReturnType} {method.Name}({parameterList1})");
            codeBuilder.AppendLine("\t{");
            if (method.ReturnType == typeof(void))
            {
                codeBuilder.AppendLine($"\t\tIoC.Resolve<SpaceBattle.Lib.ICommand>(\"SpaceShip.{method.Name}\"{parameterList2}, target).Execute();");
            }
            else codeBuilder.AppendLine($"\t\treturn IoC.Resolve<{method.ReturnType}>(\"SpaceShip.{method.Name}\"{parameterList2}, target);");
            codeBuilder.AppendLine("\t}");
        }

        codeBuilder.AppendLine("}");
        codeBuilder.AppendLine();

        return codeBuilder.ToString();
    }
}
