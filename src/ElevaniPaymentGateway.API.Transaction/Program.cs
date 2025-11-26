using AspNetCoreRateLimit;
using ElevaniPaymentGateway.Infrastructure.Extensions;
using ElevaniPaymentGateway.Infrastructure.Middlewares;
using Newtonsoft.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        //options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Host.AddAppLoggingExtension();

//Autofac
builder.Host.AddAutoFacExtension();

//Database
builder.Services.AddPersistenceExtension(builder.Configuration);

builder.Services.AddHelpersExtension();

builder.Services.AddPaymentSwaggerExtension();

builder.Services.AddPaymentJWTAuthenticationExtension(builder.Configuration);

builder.Services.AddConfigurationExtensions(builder.Configuration);

builder.Services.AddRepositoryExtension();

builder.Services.AddPaymentServiceMiddlewareExtension();

builder.Services.AddHttpContextAccessor();

builder.Services.RegisterServicesHttpProxy();

builder.Services.RegisterServicesHttpProxyClient(builder.Configuration);

builder.Services.AddPaymentRateLimiting(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DefaultModelsExpandDepth(-1);
    });
}

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.DefaultModelsExpandDepth(-1);
});

app.UseIpRateLimiting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<IPAddressWhitelistMiddleware>();

app.UseMiddleware<ResponseHeaderMiddleware>();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseMiddleware<AESEncryptionMiddleware>();

app.MapControllers();

app.Run();
