using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.DAL.Entities
{
    public class TenantEntity : KeyedEntity<Guid>, IKeyedCreateUpdateInfoEntity<UserEntity>, IKeyedDeleteInfoEntity<UserEntity>
    {
        public TenantEntity()
        {
            Id = GuidHelper.NewSequentialGuid();
        }

        [SetsRequiredMembers]
        public TenantEntity(string name, string code)
        {
            Id = GuidHelper.NewSequentialGuid();
            Name = name;
            Code = code;
        }

        public required string Name { get; set; }

        public required string Code { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public Guid CreatedById { get; set; }
        public UserEntity CreatedBy { get; set; } = default!;

        public UserEntity? UpdatedBy { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        public Guid? UpdatedById { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public UserEntity? DeletedBy { get; set; }
        public Guid? DeletedById { get; set; }

        public virtual ICollection<UserEntity> Users { get; set; } = new HashSet<UserEntity>();

        public virtual ICollection<TenantServiceEntity> TenantServices { get; set; } = new HashSet<TenantServiceEntity>();
        public virtual ICollection<TenantProductEntity> TenantProducts { get; set; } = new HashSet<TenantProductEntity>();
    }
}
