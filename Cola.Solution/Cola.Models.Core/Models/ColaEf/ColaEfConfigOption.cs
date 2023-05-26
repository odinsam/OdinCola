namespace Cola.Models.Core.Models.ColaEf;

public class ColaEfConfigOption
{
    public string TenantResolutionStrategy { get; set; } = "NoTenant";
    public List<ColaEfConfig>? ColaOrmConfig { get; set; }
}