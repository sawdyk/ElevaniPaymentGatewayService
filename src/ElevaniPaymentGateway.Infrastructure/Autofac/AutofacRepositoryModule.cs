using Autofac;

namespace ElevaniPaymentGateway.Infrastructure.Autofac
{
    public class AutofacRepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IAutoDependencyRepository).Assembly)
                .AssignableTo<IAutoDependencyRepository>()
                .As<IAutoDependencyRepository>()
                .AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
