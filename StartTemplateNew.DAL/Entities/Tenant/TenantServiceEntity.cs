using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace StartTemplateNew.DAL.Entities.Tenant
{
    public class TenantServiceEntity : KeyedEntity<int>, IKeyedTenantEntity<TenantEntity, Guid>, IKeyedActivationExpiringInfoEntity<UserEntity>
    {
        public TenantServiceEntity() { }

        public TenantServiceEntity(ServiceEntity? entity)
        {
            Service = entity;
        }

        public TenantServiceEntity(Guid? entityId)
        {
            ServiceId = entityId;
        }

        public virtual TenantEntity? Tenant { get; set; }
        public Guid? TenantId { get; set; }

        public virtual ServiceEntity? Service { get; set; }
        public Guid? ServiceId { get; set; }

        public DateTimeOffset? ActivationDate { get; set; }
        [ForeignKey(nameof(ActivationSetBy))]
        public Guid? ActivationSetById { get; set; }
        public virtual UserEntity? ActivationSetBy { get; set; }

        public DateTimeOffset? ExpirationDate { get; set; }
        [ForeignKey(nameof(ExpirationSetBy))]
        public Guid? ExpirationSetById { get; set; }
        public virtual UserEntity? ExpirationSetBy { get; set; }
    }
}
