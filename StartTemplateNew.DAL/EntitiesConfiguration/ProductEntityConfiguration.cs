using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StartTemplateNew.DAL.Entities;

namespace StartTemplateNew.DAL.EntitiesConfiguration
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder
                .HasOne(x => x.Service)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.Name).IsUnique().HasFilter("[Name] IS NOT NULL");
            builder.HasIndex(x => x.Code).IsUnique().HasFilter("[Code] IS NOT NULL");
            builder.HasIndex(x => x.NiceUrl).IsUnique().HasFilter("[NiceUrl] IS NOT NULL");
        }
    }
}
