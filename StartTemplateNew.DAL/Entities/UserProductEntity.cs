using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace StartTemplateNew.DAL.Entities
{
    public class UserProductEntity : IKeyedEntity<int>, IKeyedCreateUpdateInfoEntity<UserEntity>, IKeyedDeleteInfoEntity<UserEntity>, IKeyedExpiringInfoEntity<UserEntity>
    {
        public UserProductEntity() { }

        public UserProductEntity(Guid userId, Guid productId)
        {
            UserId = userId;
            ProductId = productId;
        }

        public UserProductEntity(Guid userId, Guid productId, DateTimeOffset? expirationDate)
        {
            UserId = userId;
            ProductId = productId;
            ExpirationDate = expirationDate;
        }

        public UserProductEntity(UserEntity user, ProductEntity product)
        {
            User = user;
            Product = product;
        }


        public UserProductEntity(UserEntity user, ProductEntity product, DateTimeOffset? expirationDate)
        {
            User = user;
            Product = product;
            ExpirationDate = expirationDate;
        }

        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual UserEntity User { get; set; } = default!;

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public virtual ProductEntity Product { get; set; } = default!;

        public DateTimeOffset? ExpirationDate { get; set; }
        public Guid? ExpirationSetById { get; set; }
        public virtual UserEntity? ExpirationSetBy { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public Guid CreatedById { get; set; }
        public virtual UserEntity CreatedBy { get; set; } = default!;

        public DateTimeOffset? UpdatedAt { get; set; }
        public Guid? UpdatedById { get; set; }
        public virtual UserEntity? UpdatedBy { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedById { get; set; }
        public virtual UserEntity? DeletedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
