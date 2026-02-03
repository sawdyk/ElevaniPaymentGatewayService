using Autofac;
using Autofac.Extensions.DependencyInjection;
using ElevaniPaymentGateway.Infrastructure.Autofac;
using ElevaniPaymentGateway.Infrastructure.Extensions;
using ElevaniPaymentGateway.Worker.Gratip;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddWindowsService();

builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
.ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext());

//Autofac
builder.ConfigureContainer(new AutofacServiceProviderFactory(), (builder) =>
{
    builder.RegisterModule(new AutofacContainerServicesModule());
    builder.RegisterModule(new AutofacRepositoryModule());
});

builder.Services.AddScoped(typeof(Handler));

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddHelpers();

builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddRepository();

builder.Services.RegisterServicesHttpProxy();

builder.Services.RegisterServicesHttpProxyClient(builder.Configuration);

var host = builder.Build();
host.Run();
