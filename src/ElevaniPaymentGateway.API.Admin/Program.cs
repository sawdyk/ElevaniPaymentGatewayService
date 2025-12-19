using AspNetCoreRateLimit;
using ElevaniPaymentGateway.Infrastructure.Extensions;
using ElevaniPaymentGateway.Infrastructure.Middlewares;
using ElevaniPaymentGateway.Persistence.DbInitializer;
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

builder.Host.AddAppLogging();

builder.Services.AddAdminCORSPolicy();

//Autofac
builder.Host.AddAutoFac();

//Database
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddHelpers();

builder.Services.AddAdminSwagger();

builder.Services.AddAdminJWTAuthentication(builder.Configuration);

builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddRepository();

builder.Services.AddAdminMiddleware();

builder.Services.AddHttpContextAccessor();

builder.Services.RegisterServicesHttpProxy();

builder.Services.RegisterServicesHttpProxyClient(builder.Configuration);

builder.Services.AddMerchantRateLimiting(builder.Configuration);

var app = builder.Build();
app.SeedRoles().Wait();
app.SeedSuperAdmin().Wait();

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

app.UseCors("CorsPolicy");

app.UseIpRateLimiting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ResponseHeaderMiddleware>();

app.UseMiddleware<GlobalExceptionMiddleware>();

//app.UseMiddleware<AESEncryptionMiddleware>();

app.MapControllers();

app.Run();

