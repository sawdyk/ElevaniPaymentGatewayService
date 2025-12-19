using ElevaniPaymentGateway.Infrastructure.BackgroundServices.Gratip;
using ElevaniPaymentGateway.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Host.AddAppLogging();

//Autofac
builder.Host.AddAutoFac();

//Database
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddHelpers();

builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddRepository();

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
