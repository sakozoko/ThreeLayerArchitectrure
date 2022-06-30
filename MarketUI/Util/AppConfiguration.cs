using Autofac;
using BLL.Util;
using MarketUI.Command;

namespace MarketUI.Util;

public static class AppConfiguration
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();

        builder.RegisterModule(new ServiceContainerModule());

        #region RegistrationCommands

        builder.RegisterType<LoginCommand>().As<ICommand>();
        builder.RegisterType<LoginCommand>().Named<ICommand>("li");
        builder.RegisterType<LoginCommand>().Named<ICommand>("login");

        builder.RegisterType<LogoutCommand>().As<ICommand>();
        builder.RegisterType<LogoutCommand>().Named<ICommand>("lo");
        builder.RegisterType<LogoutCommand>().Named<ICommand>("logout");

        builder.RegisterType<ModifyingOrderCommand>().As<ICommand>();
        builder.RegisterType<ModifyingOrderCommand>().Named<ICommand>("mo");
        builder.RegisterType<ModifyingOrderCommand>().Named<ICommand>("modifyorder");

        builder.RegisterType<OrderCreatingCommand>().As<ICommand>();
        builder.RegisterType<OrderCreatingCommand>().Named<ICommand>("co");
        builder.RegisterType<OrderCreatingCommand>().Named<ICommand>("createorder");

        builder.RegisterType<OrderHistoryViewCommand>().As<ICommand>();
        builder.RegisterType<OrderHistoryViewCommand>().Named<ICommand>("vo");
        builder.RegisterType<OrderHistoryViewCommand>().Named<ICommand>("vieworders");

        builder.RegisterType<ProductsViewCommand>().As<ICommand>();
        builder.RegisterType<ProductsViewCommand>().Named<ICommand>("vp");
        builder.RegisterType<ProductsViewCommand>().Named<ICommand>("viewproducts");

        builder.RegisterType<RegistrationCommand>().As<ICommand>();
        builder.RegisterType<RegistrationCommand>().Named<ICommand>("r");
        builder.RegisterType<RegistrationCommand>().Named<ICommand>("registration");

        builder.RegisterType<IncorrectCommand>().As<ICommand>();

        #endregion

        var container = builder.Build();

        var builder1 = new ContainerBuilder();
        builder1.Register(_ => container).As<IContainer>().SingleInstance();

        builder1.RegisterType<CommandFactory>().As<ICommandFactory>();

        builder1.RegisterType<ConsoleUserInterface>().AsSelf();

        return builder1.Build();
    }
}