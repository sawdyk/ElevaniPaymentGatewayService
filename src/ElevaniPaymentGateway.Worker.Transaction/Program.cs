using Autofac.Core;
using ElevaniPaymentGateway.Infrastructure.BackgroundServices.Gratip;
using ElevaniPaymentGateway.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Host.AddAppLoggingExtension();

//Autofac
builder.Host.AddAutoFacExtension();

//Database
builder.Services.AddPersistenceExtension(builder.Configuration);

builder.Services.AddHelpersExtension();

builder.Services.AddConfigurationExtensions(builder.Configuration);

builder.Services.AddRepositoryExtension();

builder.Services.RegisterServicesHttpProxy();

builder.Services.RegisterServicesHttpProxyClient(builder.Configuration);

//Background worker service
builder.Services.AddHostedService<GratipVerificationBackgroundJob>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.DefaultModelsExpandDepth(-1);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
