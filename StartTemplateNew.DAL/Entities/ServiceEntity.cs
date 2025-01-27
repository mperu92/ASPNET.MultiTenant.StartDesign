using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.DAL.Entities
{
    public class ServiceEntity : KeyedEntity<Guid>, IKeyedCreateUpdateInfoEntity<UserEntity>, IKeyedDeleteInfoEntity<UserEntity>
    {
        public ServiceEntity()
        {
            Id = GuidHelper.NewSequentialGuid();
        }

        [SetsRequiredMembers]
        public ServiceEntity(string name, string code)
        {
            Id = GuidHelper.NewSequentialGuid();
            Name = name;
            Code = code;
        }

        [StringLength(30)]
        public required string Name { get; set; }
        [StringLength(10)]
        public required string Code { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }

        public virtual ICollection<ProductEntity> Products { get; set; } = new HashSet<ProductEntity>();

        public DateTimeOffset CreatedAt { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public Guid CreatedById { get; set; }
        public virtual UserEntity CreatedBy { get; set; } = default!;

        public DateTimeOffset? UpdatedAt { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        public Guid? UpdatedById { get; set; }
        public virtual UserEntity? UpdatedBy { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        public Guid? DeletedById { get; set; }
        public virtual UserEntity? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<TenantServiceEntity> TenantServices { get; set; } = new HashSet<TenantServiceEntity>();
    }
}
