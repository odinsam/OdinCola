using Newtonsoft.Json.Serialization;

namespace Cola.CaUtils.ContractResolver;

public class ToLowerPropertyNamesContractResolver : DefaultContractResolver
{
    public ToLowerPropertyNamesContractResolver()
    {
        NamingStrategy = new NamingStrategyToLower();
    }
}