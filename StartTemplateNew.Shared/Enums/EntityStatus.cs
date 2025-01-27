using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace StartTemplateNew.Shared.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntityStatus : byte
    {
        Unknown = 0,
        Added = 1,
        Updated = 2,
        Deleted = 3
    }
}
