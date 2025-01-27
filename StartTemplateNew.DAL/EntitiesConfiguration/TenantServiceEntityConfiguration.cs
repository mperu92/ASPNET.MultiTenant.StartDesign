using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StartTemplateNew.DAL.Entities.Tenant;

namespace StartTemplateNew.DAL.EntitiesConfiguration
{
    public class TenantServiceEntityConfiguration : IEntityTypeConfiguration<TenantServiceEntity>
    {
        public void Configure(EntityTypeBuilder<TenantServiceEntity> builder)
        {
            builder.HasKey(x => new { x.TenantId, x.ServiceId });

            builder.HasOne(x => x.Tenant)
                .WithMany(x => x.TenantServices)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Service)
                .WithMany(x => x.TenantServices)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
