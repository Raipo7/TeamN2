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
        string adapter = "namespace SpaceBattle.Lib;\r\n"
        + "using Hwdtech;\r\n\r\n"
        + "public class IReceiverAdapter : IReceiver\r\n"
        + "{\r\n"
        + "\tprivate object target;\r\n"
        + "\tpublic IReceiverAdapter(object target)\r\n"
        + "\t{\r\n"
        + "\t\tthis.target = target;\r\n"
        + "\t}\r\n"
        + "\tpublic SpaceBattle.Lib.ICommand Receive()\r\n"
        + "\t{\r\n"
        + "\t\treturn IoC.Resolve<SpaceBattle.Lib.ICommand>(\"SpaceShip.Receive\", target);\r\n"
        + "\t}\r\n"
        + "\tpublic System.Boolean isEmpty()\r\n"
        + "\t{\r\n"
        + "\t\treturn IoC.Resolve<System.Boolean>(\"SpaceShip.isEmpty\", target);\r\n"
        + "\t}\r\n"
        + "}\r\n"
        + "\r\n";

        AdapterBuilderStrategy builder = new AdapterBuilderStrategy();
        string generatedAdapter = (string)builder.Execute(typeof(IReceiver));
        
        Assert.Equal(adapter, generatedAdapter);
    }
    [Fact]
    public void TestOfCorrectBuildingTheAdapterWithVoidFunctionAndParam(){
        string adapter = "namespace SpaceBattle.Lib;\r\n"
        + "using Hwdtech;\r\n\r\n"
        + "public class ISenderAdapter : ISender\r\n"
        + "{\r\n"
        + "\tprivate object target;\r\n"
        + "\tpublic ISenderAdapter(object target)\r\n"
        + "\t{\r\n"
        + "\t\tthis.target = target;\r\n"
        + "\t}\r\n"
        + "\tpublic System.Void Send(SpaceBattle.Lib.ICommand cmd)\r\n"
        + "\t{\r\n"
        + "\t\tIoC.Resolve<SpaceBattle.Lib.ICommand>(\"SpaceShip.Send\", cmd, target).Execute();\r\n"
        + "\t}\r\n"
        + "}\r\n"
        + "\r\n";

        AdapterBuilderStrategy builder = new AdapterBuilderStrategy();
        string generatedAdapter = (string)builder.Execute(typeof(ISender));
        
        Assert.Equal(adapter, generatedAdapter);
    }
}
