using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StartTemplateNew.DAL;
using StartTemplateNew.DAL.Identity;
using StartTemplateNew.DAL.Repositories;
using StartTemplateNew.DAL.TenantUserProvider;
using StartTemplateNew.DAL.UnitOfWork;
using StartTemplateNew.Shared;
using StartTemplateNew.Shared.ApiVersioning;
using StartTemplateNew.Shared.ApiVersioning.Models;
using StartTemplateNew.Shared.Exceptions;
using StartTemplateNew.Shared.Models;
using StartTemplateNew.Shared.Services;
using StartTemplateNew.WebApi;
using StartTemplateNew.Shared.Logging.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogLogging();

// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<IssuerCredentials>(builder.Configuration.GetSection("IssuerCredentials"));

string connectionString = builder.Configuration.GetConnectionString("DbConnection")
    ?? throw new AppSettingNotFoundException("Connection string not found in appsettings");

IssuerCredentials issuerCredentials = builder.Configuration.GetSection("IssuerCredentials").Get<IssuerCredentials>()
    ?? throw new AppSettingNotFoundException("IssuerCredentials section not found");

ApiVersioningInfo apiVersioningInfo = builder.Configuration.GetSection("ApiVersioningInfo").Get<ApiVersioningInfo>()
    ?? throw new AppSettingNotFoundException("ApiVersioningInfo section not found");

builder.Services
    .AddApplicationDbContext(connectionString, useLazyLoadingProxies: false)
    .AddEntityFrameworkIdentity();

builder.Services
    .AddCookieAuth()
    .AddJwtBearerAuth(issuerCredentials);

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddProviders()
    .AddClaimUserProvider()
    .AddRepositories()
    .AddUnitOfWork();

builder.Services
    .AddAppAutoMapper()
    .AddAppFluentValidation()
    .AddServices();

builder.Services.AddApplicationApiVersioning(apiVersioningInfo);

builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
