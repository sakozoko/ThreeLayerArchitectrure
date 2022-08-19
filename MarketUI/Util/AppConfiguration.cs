using Autofac;
using BLL.Util;
using MarketUI.Command;
using MarketUI.Command.Base;
using MarketUI.Command.Order;
using MarketUI.Command.Product;
using MarketUI.Command.User;
using MarketUI.Command.Util;
using MarketUI.Util.Interface;

namespace MarketUI.Util;

public static class AppConfiguration
{
    public static IContainer Configure(ICommandsInfoHandler cih)
    {
        var builder = new ContainerBuilder();

        builder.RegisterModule(new ServiceContainerModule());
        builder.RegisterType<AutoMapperHandler>().As<IUserInterfaceMapperHandler>();
        builder.RegisterInstance(cih).As<ICommandsInfoHandler>();

        #region RegistrationCommands

        builder.RegisterType<LoginCommand>().As<ICommand>();
        builder.RegisterType<LoginCommand>().Named<ICommand>(cih.GetCommandInfo(nameof(LoginCommand)).Abbreviated);
        builder.RegisterType<LoginCommand>().Named<ICommand>(cih.GetCommandInfo(nameof(LoginCommand)).FullName);

        builder.RegisterType<LogoutCommand>().As<ICommand>();
        builder.RegisterType<LogoutCommand>().Named<ICommand>(cih.GetCommandInfo(nameof(LogoutCommand)).Abbreviated);
        builder.RegisterType<LogoutCommand>().Named<ICommand>(cih.GetCommandInfo(nameof(LogoutCommand)).FullName);

        builder.RegisterType<OrderModifyingCommand>().As<ICommand>();
        builder.RegisterType<OrderModifyingCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(OrderModifyingCommand)).Abbreviated);
        builder.RegisterType<OrderModifyingCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(OrderModifyingCommand)).FullName);

        builder.RegisterType<OrderCreatingCommand>().As<ICommand>();
        builder.RegisterType<OrderCreatingCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(OrderCreatingCommand)).Abbreviated);
        builder.RegisterType<OrderCreatingCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(OrderCreatingCommand)).FullName);

        builder.RegisterType<OrderHistoryViewCommand>().As<ICommand>();
        builder.RegisterType<OrderHistoryViewCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(OrderHistoryViewCommand)).Abbreviated);
        builder.RegisterType<OrderHistoryViewCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(OrderHistoryViewCommand)).FullName);

        builder.RegisterType<ProductsViewCommand>().As<ICommand>();
        builder.RegisterType<ProductsViewCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(ProductsViewCommand)).Abbreviated);
        builder.RegisterType<ProductsViewCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(ProductsViewCommand)).FullName);

        builder.RegisterType<RegistrationCommand>().As<ICommand>();
        builder.RegisterType<RegistrationCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(RegistrationCommand)).Abbreviated);
        builder.RegisterType<RegistrationCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(RegistrationCommand)).FullName);

        builder.RegisterType<PersonalInformationChangingCommand>().As<ICommand>();
        builder.RegisterType<PersonalInformationChangingCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(PersonalInformationChangingCommand)).Abbreviated);
        builder.RegisterType<PersonalInformationChangingCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(PersonalInformationChangingCommand)).FullName);

        builder.RegisterType<ProductModifyingCommand>().As<ICommand>();
        builder.RegisterType<ProductModifyingCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(ProductModifyingCommand)).Abbreviated);
        builder.RegisterType<ProductModifyingCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(ProductModifyingCommand)).FullName);

        builder.RegisterType<ProductCreatingCommand>().As<ICommand>();
        builder.RegisterType<ProductCreatingCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(ProductCreatingCommand)).Abbreviated);
        builder.RegisterType<ProductCreatingCommand>()
            .Named<ICommand>(cih.GetCommandInfo(nameof(ProductCreatingCommand)).FullName);

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