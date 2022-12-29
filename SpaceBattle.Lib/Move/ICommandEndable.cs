namespace SpaceBattle.Lib;

public interface ICommandEnbable
{
    public IUObject UObject { get; set; }
    public IDictionary<string, object> properties { get; set; }
}
