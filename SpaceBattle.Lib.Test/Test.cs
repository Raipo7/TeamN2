namespace SpaceBattle.Lib.Test;
using Xunit;
using System.Diagnostics;
public class TEST {

    [Fact]
    public void qwe() {
        var a = new Stopwatch();
        a.Start();
        System.Threading.Thread.Sleep(500);
        System.Console.WriteLine(a.ElapsedMilliseconds);
        a.Reset();
        System.Threading.Thread.Sleep(500);
        System.Console.WriteLine(a.ElapsedMilliseconds);
        a.Stop();
    }
}
