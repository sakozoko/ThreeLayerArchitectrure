using System;
using System.Threading.Tasks;
using BLL;
using Entities;
using Shop.Command;
using Shop.Helpers;

namespace Shop;

public class Shop
{
    private static readonly ICommand IncorrectCommand = new IncorrectCommand();
    public static AuthenticateResponse AuthenticationData;
    private readonly ICommand[] _commands;
    private readonly Service _service = new();

    public Shop()
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
            IncorrectCommand
        };

        _commands[^1] = new HelpCommand(_commands);

        #endregion
    }

    private ICommand CorrectCommand(string name)
    {
        var correctCommand = IncorrectCommand;
        foreach (var command in _commands)
        {
            if (!command.ItsMe(name)) continue;
            correctCommand = command;
            break;
        }

        return correctCommand;
    }

    public void ExecuteCommand(string commandString)
    {
        var args = SplitString(commandString);
        var command = CorrectCommand(args[0]);
        var result = command.Execute(args);
        Console.WriteLine(result);
    }

    private string[] SplitString(string str)
    {
        return ParseStringHelper.SplitStringWithQuotes(str);
    }
}