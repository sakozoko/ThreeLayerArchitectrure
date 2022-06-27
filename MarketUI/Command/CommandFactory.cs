using System.Linq;
using BLL;

namespace MarketUI.Util.Command;

public class CommandFactory : ICommandFactory
{
    private static readonly ICommand IncorrectCommand = new IncorrectCommand();
    private readonly ICommand[] _commands;

    public CommandFactory(IServiceContainer service)
    {
        #region SetCommands

        _commands = new[]
        {
            new ProductsViewCommand(service),
            new LoginCommand(service),
            new LogoutCommand(),
            new OrderCreatingCommand(service),
            new OrderHistoryViewCommand(service),
            new RegistrationCommand(service),
            new ModifyingOrderCommand(service),
            IncorrectCommand
        };

        _commands[^1] = new HelpCommand(_commands);

        #endregion
    }

    public ICommand GetCommand(string name)
    {
        var returnCommand = IncorrectCommand;
        foreach (var command in _commands)
        {
            if (!((command as BaseCommand)?.Names?.Any(x => x.Contains(name)) ?? false)) continue;
            returnCommand = command;
            break;
        }

        return returnCommand;
    }
}