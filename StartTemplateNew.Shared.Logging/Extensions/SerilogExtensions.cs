using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StartTemplateNew.Shared.Logging.Configurations;
using Serilog;

namespace StartTemplateNew.Shared.Logging.Extensions
{
    public static class SerilogExtensions
    {
        public static IHostBuilder UseSerilogLogging(this IHostBuilder builder)
        {
            SerilogConfig.ConfigureSerilog();
            builder.UseSerilog(); // This method is defined in Serilog.Extensions.Hosting
            return builder;
        }

        public static IServiceCollection AddSerilogLogging(this IServiceCollection services)
        {
            SerilogConfig.ConfigureSerilog();

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            return services;
        }
    }
}
