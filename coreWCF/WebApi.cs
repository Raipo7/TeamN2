using CoreWCF;
using Hwdtech;

namespace WCF;

[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
internal class WebApi : IWebApi
{
    public void GetMessage(MessageContract param)
    {
        try
        {
            var threadId = IoC.Resolve<string>("Thread.GetIdByGameId", param.gameId);
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Thread.SendCommand", threadId, IoC.Resolve<SpaceBattle.Lib.ICommand>("Create.CommandByMessage", param)).Execute();  //7.2 - обработка сообщений
        }
        catch (System.Exception err)
        {
            System.Console.WriteLine("\n" + err + "\n");
        }
    }
}
