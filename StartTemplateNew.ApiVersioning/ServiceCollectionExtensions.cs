using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;
using StartTemplateNew.Shared.ApiVersioning.Models;

namespace StartTemplateNew.Shared.ApiVersioning
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationApiVersioning(this IServiceCollection services, ApiVersioningInfo apiVersioningInfo)
        {
            ArgumentNullException.ThrowIfNull(apiVersioningInfo);

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(majorVersion: apiVersioningInfo.CurrentApiVersion); // currently 1.0
                options.AssumeDefaultVersionWhenUnspecified = apiVersioningInfo.AssumeDefaultVersionWhenUnspecified;
                options.ReportApiVersions = apiVersioningInfo.ReportApiVersions;

                options.ApiVersionReader = new UrlSegmentApiVersionReader(); // ApiVersionReader.Combine(...many readers)
            });

            return services;
        }
    }
}
