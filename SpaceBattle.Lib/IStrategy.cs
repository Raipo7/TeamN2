namespace SpaceBattle.Lib;

public interface IStrategy
{
    public object Execute(params object[] arg);
}
