using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using StartTemplateNew.Shared.Models;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using StartTemplateNew.Shared.FluentValidation.Factories;
using System.Reflection;
using StartTemplateNew.Shared.Providers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StartTemplateNew.Shared.Providers.Impl;
using StartTemplateNew.DAL.TenantUserProvider.Core.Impl;
using StartTemplateNew.DAL.TenantUserProvider.Core;

namespace StartTemplateNew.Shared
{
    [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>")]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCookieAuth(this IServiceCollection services)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddCookie(IdentityConstants.ApplicationScheme, o =>
                {
                    //o.LoginPath = new PathString("/Account/Login");
                    //o.AccessDeniedPath = new PathString("/Account/AccessDenied");
                    o.Events = new CookieAuthenticationEvents()
                    {
                        OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync,
                        OnRedirectToAccessDenied = context =>
                        {
                            context.Response.StatusCode = 403;
                            return Task.CompletedTask;
                        }
                    };
                });
            //.AddCookie(IdentityConstants.ExternalScheme, o =>
            //{
            //    o.Cookie.Name = IdentityConstants.ExternalScheme;
            //    o.AccessDeniedPath = new PathString("/Account/AccessDenied");
            //    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            //})
            //.AddCookie(IdentityConstants.TwoFactorRememberMeScheme, o =>
            //{
            //    o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
            //    o.AccessDeniedPath = new PathString("/Account/AccessDenied");
            //    o.Events = new CookieAuthenticationEvents
            //    {
            //        OnValidatePrincipal = SecurityStampValidator.ValidateAsync<ITwoFactorSecurityStampValidator>
            //    };
            //})
            //.AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
            //{
            //    o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
            //    o.AccessDeniedPath = new PathString("/Account/AccessDenied");
            //    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            //});

            return services;
        }

        public static IServiceCollection AddJwtBearerAuth(this IServiceCollection services, IssuerCredentials issuerCredentials)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = issuerCredentials.ValidateIssuer,
                        ValidateAudience = issuerCredentials.ValidateAudience,
                        ValidateLifetime = issuerCredentials.ValidateLifetime,
                        ValidateIssuerSigningKey = issuerCredentials.ValidateIssuerSigningKey,
                        ValidIssuer = issuerCredentials.Issuer,
                        ValidAudiences = issuerCredentials.AudienceList,
                        IssuerSigningKey = new SymmetricSecurityKey(issuerCredentials.SigningKeyBytes)
                    };
                });

            return services;
        }

        public static IServiceCollection AddAppFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

            services.AddFluentValidationAutoValidation(configuration =>
            {
                // Disable the built-in .NET model (data annotations) validation.
                configuration.DisableBuiltInModelValidation = true;

                // Only validate controllers decorated with the `FluentValidationAutoValidation` attribute.
                configuration.ValidationStrategy = ValidationStrategy.All;

                // Enable validation for parameters bound from `BindingSource.Body` binding sources.
                configuration.EnableBodyBindingSourceAutomaticValidation = true;

                // Enable validation for parameters bound from `BindingSource.Form` binding sources.
                configuration.EnableFormBindingSourceAutomaticValidation = true;

                // Enable validation for parameters bound from `BindingSource.Query` binding sources.
                configuration.EnableQueryBindingSourceAutomaticValidation = true;

                // Enable validation for parameters bound from `BindingSource.Path` binding sources.
                configuration.EnablePathBindingSourceAutomaticValidation = true;

                // Enable validation for parameters bound from 'BindingSource.Custom' binding sources.
                configuration.EnableCustomBindingSourceAutomaticValidation = true;

                // Replace the default result factory with a custom implementation.
                configuration.OverrideDefaultResultFactoryWith<FluentValidationResultFactory>();
            });

            return services;
        }

        public static IServiceCollection AddAppAutoMapper(this IServiceCollection services)
        {
            Assembly[] ass = AppDomain.CurrentDomain.GetAssemblies();

            services.AddAutoMapper(ass);

            return services;
        }

        public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            IEnumerable<Type> providerInterfaces = assembly.GetTypes()
                .Where(t => t.IsInterface && t.GetInterfaces().Contains(typeof(IProvider)));

            IEnumerable<Type> providerImplementations = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IProvider)));

            foreach (Type providerInterface in providerInterfaces)
            {
                Type? implementation = providerImplementations.FirstOrDefault(t => t.GetInterfaces().Contains(providerInterface));
                if (implementation != null)
                    services.AddScoped(providerInterface, implementation);
            }

            return services;
        }
    }
}
