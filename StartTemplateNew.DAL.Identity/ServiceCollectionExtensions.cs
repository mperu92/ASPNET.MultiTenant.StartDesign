using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Identity.Stores;

namespace StartTemplateNew.DAL.Identity
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<UserEntity>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters += "!_"; // Add the special characters you want to allow
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
            .AddRoles<RoleEntity>()
            .AddUserStore<ApplicationUserStore>()
            .AddRoleStore<ApplicationRoleStore>()
            .AddUserManager<UserManager<UserEntity>>()
            .AddRoleManager<RoleManager<RoleEntity>>()
            .AddSignInManager<SignInManager<UserEntity>>();

            return services;
        }
    }
}
