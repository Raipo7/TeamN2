using CoreWCF;
using Hwdtech;

namespace WCF;

[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
internal class WebApi : IWebApi
{
    public void GetMessage(MessageContract param) {
        try {

            System.Console.WriteLine(param.properties.Count);
            //IoC.Resolve<SpaceBattle.Lib.ICommand>("WCF.SendMessage", param).Execute();  //7.2 - обработка сообщений
        }
        catch(System.Exception err) {
            System.Console.WriteLine("\n" + err + "\n");
        }
    }
}
