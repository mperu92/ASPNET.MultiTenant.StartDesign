using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Entities.Tenant;
using System.Reflection;

namespace StartTemplateNew.DAL.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<UserEntity, RoleEntity, Guid, UserClaimEntity, UserRoleEntity, UserLoginEntity, RoleClaimEntity, UserTokenEntity>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<TenantEntity> Tenants { get; set; } = default!;
        public DbSet<TenantServiceEntity> TenantServices { get; set; } = default!;
        public DbSet<TenantProductEntity> TenantProducts { get; set; } = default!;
        public DbSet<UserProductEntity> UserProducts { get; set; } = default!;

        public DbSet<ServiceEntity> Services { get; set; } = default!;
        public DbSet<ProductEntity> Products { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            #region identity
            builder.Entity<UserRoleEntity>(builder =>
            {
                builder.HasKey(ur => new { ur.UserId, ur.RoleId });

                builder
                    .HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                builder
                    .HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserEntity>(builder =>
            {
                builder.HasIndex(u => u.NormalizedUserName).IsUnique();
                builder.HasIndex(u => u.UserName).IsUnique();
                builder.HasIndex(u => u.NormalizedEmail);
                builder.HasIndex(u => u.Email);

                builder.HasOne(u => u.Tenant)
                    .WithMany(t => t.Users)
                    .HasForeignKey(u => u.TenantId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<RoleEntity>(builder =>
            {
                builder.HasIndex(r => r.NormalizedName).IsUnique();
                builder.HasIndex(r => r.Name).IsUnique();
            });

            builder.Entity<UserClaimEntity>(builder =>
            {
                builder.HasOne(uc => uc.User)
                    .WithMany(u => u.UserClaims)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserLoginEntity>(builder =>
            {
                builder.HasOne(ul => ul.User)
                    .WithMany(u => u.UserLogins)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserTokenEntity>(builder => {
                builder.HasOne(ut => ut.User)
                    .WithMany(u => u.UserTokens)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<RoleClaimEntity>(builder => {
                builder.HasOne(rc => rc.Role)
                    .WithMany(r => r.RoleClaims)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion
        }
    }
}
