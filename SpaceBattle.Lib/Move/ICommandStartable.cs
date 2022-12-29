namespace SpaceBattle.Lib;

public interface ICommandStartable
{
    public IUObject UObject { get; set; }
    public IDictionary<string, object> properties { get; set; }
}
