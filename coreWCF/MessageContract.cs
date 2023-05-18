using System.Collections.Generic;
using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;

using SpaceBattle.Lib;
namespace WCF;

[DataContract(Name = "MessageContract", Namespace = "http://spacebattle.com")]
internal class MessageContract : IMessage
{
    [DataMember(Name = "type", Order = 1)]
    [OpenApiProperty(Description = "Игровой тип комманды")]
    public string type { get; set; }
    [DataMember(Name = "game id", Order = 2)]
    [OpenApiProperty(Description = "ID потока/игры")]
    public string gameId { get; set; }
    [DataMember(Name = "game item id", Order = 3)]
    [OpenApiProperty(Description = "ID игрового объекта")]
    public string gameItemId { get; set; }
    [DataMember(Name = "properties", Order = 4)]
    [OpenApiProperty(Description = "Дополнительные параметры")]
    public JsonDictionary innerDict { get; set; } //для обработки внутреннего словаря в json
    
    public IDictionary<string, object> properties
    {
        get
        {
            return (IDictionary<string, object>)innerDict.dict;
        }
    }
}
