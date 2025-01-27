using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StartTemplateNew.DAL.Entities.Tenant;

namespace StartTemplateNew.DAL.EntitiesConfiguration
{
    public class TenantProductEntityConfiguration : IEntityTypeConfiguration<TenantProductEntity>
    {
        public void Configure(EntityTypeBuilder<TenantProductEntity> builder)
        {
            builder.HasKey(x => new { x.TenantId, x.ProductId });

            builder.HasOne(x => x.Tenant)
                .WithMany(x => x.TenantProducts)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.TenantProducts)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
