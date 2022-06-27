using System.Linq;
using BLL.Services.Factory;

namespace Shop.Command;

public class CommandFactory :ICommandFactory
{
    private static readonly ICommand IncorrectCommand = new IncorrectCommand();
    private readonly ICommand[] _commands;
    private readonly IServiceContainer _service = new ServiceContainer();

    public CommandFactory()
    {
        #region SetCommands
        _commands = new[]
        {
            new ProductsViewCommand(_service),
            new LoginCommand(_service),
            new LogoutCommand(),
            new OrderCreatingCommand(_service),
            new OrderHistoryViewCommand(_service),
            new RegistrationCommand(_service),
            new ModifyingOrderCommand(_service),
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