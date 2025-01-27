using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StartTemplateNew.DAL.Entities;

namespace StartTemplateNew.DAL.EntitiesConfiguration
{
    public class ServiceEntityConfiguration : IEntityTypeConfiguration<ServiceEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceEntity> builder)
        {
            builder.HasMany(x => x.Products)
                .WithOne(x => x.Service)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.Name).IsUnique().HasFilter("[Name] IS NOT NULL");
            builder.HasIndex(x => x.Code).IsUnique().HasFilter("[Code] IS NOT NULL");
        }
    }
}
