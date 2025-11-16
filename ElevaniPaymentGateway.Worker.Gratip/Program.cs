using ElevaniPaymentGateway.Infrastructure.Extensions;
using ElevaniPaymentGateway.Infrastructure.Implementations.ProxyClients.Gratip;
using ElevaniPaymentGateway.Infrastructure.Implementations.Queries;
using ElevaniPaymentGateway.Infrastructure.Implementations.Services.Utilities;
using ElevaniPaymentGateway.Infrastructure.Interfaces.ProxyClients.Gratip;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Queries;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using ElevaniPaymentGateway.Persistence;
using ElevaniPaymentGateway.Worker.Gratip;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddWindowsService();

builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
.ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext());

builder.Services.AddScoped(typeof(Handler));

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("PGDatabaseCon"))
    .EnableSensitiveDataLogging(false);
});

builder.Services.AddConfigurationExtensions(builder.Configuration);

builder.Services.AddScoped<ITransactionQuery, TransactionQuery>();
builder.Services.AddScoped<IGratipTransactionQuery, GratipTransactionQuery>();
builder.Services.AddScoped<IGratipCollectionService, GratipCollectionService>();
builder.Services.AddScoped<ISqlTransactionService, SqlTransactionService>();


builder.Services.AddRepositoryExtension();

builder.Services.RegisterServicesHttpProxy();

builder.Services.RegisterServicesHttpProxyClient(builder.Configuration);

var host = builder.Build();
host.Run();
