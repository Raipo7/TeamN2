namespace SpaceBattle.Lib;

public interface IAdapterBuilder
{
    IAdapterBuilder AddNamespace(string nameSpace);
    IAdapterBuilder AddUsings(string[] usings);
    IAdapterBuilder AddClassName();
    IAdapterBuilder InitializationObject();
    IAdapterBuilder AddConstructor();
    IAdapterBuilder AddMethods();
    IAdapterBuilder CloseClass();
    public string Build();
}
