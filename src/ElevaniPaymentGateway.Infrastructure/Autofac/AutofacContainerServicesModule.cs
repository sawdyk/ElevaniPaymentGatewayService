using Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Autofac
{
    public class AutofacContainerServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IAutoDependencyServices).Assembly)
                   .AssignableTo<IAutoDependencyServices>()
                   .As<IAutoDependencyServices>()
                   .AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
