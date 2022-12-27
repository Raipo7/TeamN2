namespace SpaceBattle.Lib.Test;
using Xunit;

public class TestEmptyCommand
{
    [Fact]
    public void EmptyCommandExecute()
    {
        new EmptyCommand().Execute();
    }
}
