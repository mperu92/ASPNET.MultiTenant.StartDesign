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
    public class ProductEntity : KeyedEntity<Guid>, IKeyedCreateUpdateInfoEntity<UserEntity>, IKeyedDeleteInfoEntity<UserEntity>
    {
        public ProductEntity()
        {
            Id = GuidHelper.NewSequentialGuid();
        }

        [SetsRequiredMembers]
        public ProductEntity(string code, string name, string shortDescription, string niceUrl)
        {
            Id = GuidHelper.NewSequentialGuid();
            Code = code;
            Name = name;
            ShortDescription = shortDescription;
            NiceUrl = niceUrl;
        }

        [StringLength(10)]
        public required string Code { get; set; }
        [StringLength(50)]
        public required string Name { get; set; }
        [StringLength(150)]
        public required string ShortDescription { get; set; }
        [StringLength(20)]
        public required string NiceUrl { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [ForeignKey(nameof(Service))]
        public Guid ServiceId { get; set; }
        public virtual ServiceEntity Service { get; set; } = default!;

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

        public virtual ICollection<TenantProductEntity> TenantProducts { get; set; } = new HashSet<TenantProductEntity>();
    }
}
