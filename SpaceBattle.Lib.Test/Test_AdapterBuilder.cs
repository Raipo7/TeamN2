namespace SpaceBattle.Lib.Test;
using Xunit;
using Hwdtech;

public class Test_AdapterBuilder
{
    public object globalScope;        
    public Test_AdapterBuilder() {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Interface.GetParameterList", (object[] args) => {
            return new GetParameterListStrategy().Execute(args[0], args[1]);
        }).Execute();        

        globalScope = scope;
    }
    [Fact]
    public void TestOfCorrectBuildingTheAdapterWithoutVoidFunctionAndParam(){
        string adapter = "namespace SpaceBattle.Lib;\n"
        + "using Hwdtech;\n\n"
        + "public class IReceiverAdapter : IReceiver\n"
        + "{\n"
        + "\tprivate object target;\n"
        + "\tpublic IReceiverAdapter(object target)\n"
        + "\t{\n"
        + "\t\tthis.target = target;\n"
        + "\t}\n"
        + "\tpublic SpaceBattle.Lib.ICommand Receive()\n"
        + "\t{\n"
        + "\t\treturn IoC.Resolve<SpaceBattle.Lib.ICommand>(\"SpaceShip.Receive\", target);\n"
        + "\t}\n"
        + "\tpublic System.Boolean isEmpty()\n"
        + "\t{\n"
        + "\t\treturn IoC.Resolve<System.Boolean>(\"SpaceShip.isEmpty\", target);\n"
        + "\t}\n"
        + "}\n"
        + "\n";

        string generatedAdapter = new AdapterBuilder(typeof(IReceiver))
        .AddNamespace("SpaceBattle.Lib")
        .AddUsings(new string[] {"Hwdtech"})
        .AddClassName()
        .InitializationObject()
        .AddConstructor()
        .AddMethods()
        .CloseClass()
        .Build();
        
        Assert.Equal(adapter, generatedAdapter);
    }
   [Fact]
    public void TestOfCorrectBuildingTheAdapterWithVoidFunctionAndParam(){
        string adapter = "namespace SpaceBattle.Lib;\n"
        + "using Hwdtech;\n\n"
        + "public class ISenderAdapter : ISender\n"
        + "{\n"
        + "\tprivate object target;\n"
        + "\tpublic ISenderAdapter(object target)\n"
        + "\t{\n"
        + "\t\tthis.target = target;\n"
        + "\t}\n"
        + "\tpublic System.Void Send(SpaceBattle.Lib.ICommand cmd)\n"
        + "\t{\n"
        + "\t\tIoC.Resolve<SpaceBattle.Lib.ICommand>(\"SpaceShip.Send\", cmd, target).Execute();\n"
        + "\t}\n"
        + "}\n"
        + "\n";

        string generatedAdapter = new AdapterBuilder(typeof(ISender))
        .AddNamespace("SpaceBattle.Lib")
        .AddUsings(new string[] {"Hwdtech"})
        .AddClassName()
        .InitializationObject()
        .AddConstructor()
        .AddMethods()
        .CloseClass()
        .Build();
        
        Assert.Equal(adapter, generatedAdapter);
    }
}
