using Autofac;
using BLL.Util;
using MarketUI.Command;
using MarketUI.Command.Base;
using MarketUI.Command.Category;
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

        #region CommandsRegistration

        #region UserCommand

        builder.RegisterType<LoginCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<LoginCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(LoginCommand)).Abbreviated);
        builder.RegisterType<LoginCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(LoginCommand)).FullName);

        builder.RegisterType<LogoutCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<LogoutCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(LogoutCommand)).Abbreviated);
        builder.RegisterType<LogoutCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(LogoutCommand)).FullName);

        builder.RegisterType<RegistrationCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<RegistrationCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(RegistrationCommand)).Abbreviated);
        builder.RegisterType<RegistrationCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(RegistrationCommand)).FullName);

        builder.RegisterType<PersonalInformationChangingCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<PersonalInformationChangingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(PersonalInformationChangingCommand)).Abbreviated);
        builder.RegisterType<PersonalInformationChangingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(PersonalInformationChangingCommand)).FullName);

        #endregion

        #region OrderCommands

        builder.RegisterType<OrderModifyingCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<OrderModifyingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(OrderModifyingCommand)).Abbreviated);
        builder.RegisterType<OrderModifyingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(OrderModifyingCommand)).FullName);

        builder.RegisterType<OrderCreatingCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<OrderCreatingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(OrderCreatingCommand)).Abbreviated);
        builder.RegisterType<OrderCreatingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(OrderCreatingCommand)).FullName);

        builder.RegisterType<OrderHistoryViewCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<OrderHistoryViewCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(OrderHistoryViewCommand)).Abbreviated);
        builder.RegisterType<OrderHistoryViewCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(OrderHistoryViewCommand)).FullName);

        #endregion

        #region ProductCommands

        builder.RegisterType<ProductsViewCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<ProductsViewCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(ProductsViewCommand)).Abbreviated);
        builder.RegisterType<ProductsViewCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(ProductsViewCommand)).FullName);

        builder.RegisterType<ProductModifyingCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<ProductModifyingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(ProductModifyingCommand)).Abbreviated);
        builder.RegisterType<ProductModifyingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(ProductModifyingCommand)).FullName);

        builder.RegisterType<ProductCreatingCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<ProductCreatingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(ProductCreatingCommand)).Abbreviated);
        builder.RegisterType<ProductCreatingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(ProductCreatingCommand)).FullName);

        #endregion

        #region CategoryCommands

        builder.RegisterType<CategoryCreatingCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<CategoryCreatingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(CategoryCreatingCommand)).Abbreviated);
        builder.RegisterType<CategoryCreatingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(CategoryCreatingCommand)).FullName);

        builder.RegisterType<CategoryModifyingCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<CategoryModifyingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(CategoryModifyingCommand)).Abbreviated);
        builder.RegisterType<CategoryModifyingCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(CategoryModifyingCommand)).FullName);

        builder.RegisterType<CategoryViewCommand>().As<IDescriptiveCommand>();
        builder.RegisterType<CategoryViewCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(CategoryViewCommand)).Abbreviated);
        builder.RegisterType<CategoryViewCommand>()
            .Named<IExecutableCommand>(cih.GetCommandInfo(nameof(CategoryViewCommand)).FullName);

        #endregion

        builder.RegisterType<IncorrectCommand>().As<IExecutableCommand>();

        #endregion

        var container = builder.Build();

        var builder1 = new ContainerBuilder();
        builder1.Register(_ => container).As<IContainer>().SingleInstance();

        builder1.RegisterType<CommandFactory>().As<ICommandFactory>();

        builder1.RegisterType<ConsoleUserInterface>().AsSelf();

        return builder1.Build();
    }
}