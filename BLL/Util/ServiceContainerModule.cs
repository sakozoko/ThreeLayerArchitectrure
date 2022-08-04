using Autofac;
using BLL.Helpers.Token;
using BLL.Util.Interface;
using BLL.Util.Logger;
using DAL.Util;

namespace BLL.Util;

public class ServiceContainerModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule(new UnitOfWorkModule());
        builder.RegisterType<AutoMapperHandler>().As<IDomainMapperHandler>().SingleInstance();
        builder.RegisterType<DebugLogger>().As<ILogger>();
        builder.RegisterType<CustomTokenHandler>().As<ITokenHandler>();
        builder.RegisterType<ServiceContainer>().As<IServiceContainer>();
    }
}