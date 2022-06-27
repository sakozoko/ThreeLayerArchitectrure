using Autofac;
using BLL.Util;
using MarketUI.Util.Command;

namespace MarketUI.Util;

public static class AppConfiguration
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule(new ServiceContainerModule());
        builder.RegisterType<CommandFactory>().As<ICommandFactory>();
        builder.RegisterType<ConsoleUserInterface>().AsSelf();
        return builder.Build();
    }
}