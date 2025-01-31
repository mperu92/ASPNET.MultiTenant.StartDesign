using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace StartTemplateNew.DAL.Entities.Tenant
{
    public class TenantProductEntity : KeyedEntity<int>, IKeyedTenantEntity<TenantEntity, Guid>, IKeyedActivationExpiringInfoEntity<UserEntity>
    {
        public TenantProductEntity() { }

        public TenantProductEntity(ProductEntity? entity)
        {
            Product = entity;
        }

        public TenantProductEntity(Guid? entityId)
        {
            ProductId = entityId;
        }

        public virtual TenantEntity? Tenant { get; set; }
        public Guid? TenantId { get; set; }

        public virtual ProductEntity? Product { get; set; }
        public Guid? ProductId { get; set; }

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
