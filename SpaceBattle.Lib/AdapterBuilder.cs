namespace SpaceBattle.Lib;
using System;
using Hwdtech;
using System.Reflection;
using System.Text;

public class AdapterBuilder : IAdapterBuilder
{   
    private Type interfaceType;
    private StringBuilder codeBuilder;

    public AdapterBuilder(Type interfaceType)
    {
        this.interfaceType = interfaceType;
        codeBuilder = new StringBuilder();
    }

    public IAdapterBuilder AddNamespace(string nameSpace)
    {
        codeBuilder.AppendLine($"namespace {nameSpace};");
        return this;
    }
    public IAdapterBuilder AddUsings(string[] usings)
    {   
        foreach (string use in usings)
        {
            codeBuilder.AppendLine($"using {use};");
        }
        codeBuilder.AppendLine();
        return this;
    }
    public IAdapterBuilder AddClassName()
    {
        codeBuilder.AppendLine($"public class {interfaceType.Name}Adapter : {interfaceType.Name}");
        codeBuilder.AppendLine("{");
        return this;
    }
    public IAdapterBuilder InitializationObject()
    {
        codeBuilder.AppendLine("\tprivate object target;");
        return this;
    }
    public IAdapterBuilder AddConstructor()
    {
        codeBuilder.AppendLine($"\tpublic {interfaceType.Name}Adapter(object target)");
        codeBuilder.AppendLine("\t{");
        codeBuilder.AppendLine("\t\tthis.target = target;");
        codeBuilder.AppendLine("\t}");
        return this;
    }
    public IAdapterBuilder AddMethods()
    {
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
        return this;
    }
    public IAdapterBuilder CloseClass()
    {
        codeBuilder.AppendLine("}");
        codeBuilder.AppendLine();
        return this;
    }
    public string Build()
    {
        return codeBuilder.ToString();
    }
}
