using Autofac;
using MarketUI.Command.Base;
using MarketUI.Command.Util;

namespace MarketUI.Command;

public class CommandFactory : ICommandFactory
{
    private readonly IContainer _container;
    private readonly ICommand _helpCommand;

    public CommandFactory(IContainer container)
    {
        #region SetCommands

        _container = container;
        _helpCommand = new HelpCommand(_container);

        #endregion
    }

    public ICommand GetCommand(string name)
    {
        return name is "h" or "?" or "help"
            ? _helpCommand
            : _container.ResolveOptionalNamed<ICommand>(name) ?? _container.Resolve<ICommand>();
    }
}