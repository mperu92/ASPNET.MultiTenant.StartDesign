using StartTemplateNew.DAL.Entities.Base;

namespace StartTemplateNew.DAL.Entities.Tenant
{
    public interface INullableKeyedTenantEntity<TTenant> : IEntity
        where TTenant : class, IEntity
    {
        TTenant? Tenant { get; set; }
    }
}
