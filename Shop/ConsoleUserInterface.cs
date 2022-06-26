using System;
using BLL;
using BLL.Services.Exception;
using BLL.Services.Factory;
using Entities;
using Shop.Command;
using Shop.Helpers;

namespace Shop;

public class ConsoleUserInterface
{
    private static readonly ICommand IncorrectCommand = new IncorrectCommand();
    private readonly ICommand[] _commands;
    private readonly IServiceContainer _service = new ServiceContainer();

    public ConsoleUserInterface()
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

    public static AuthenticateResponse AuthenticationData { get; set; }

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
        try
        {
            var result = command.Execute(args);
            WriteMessage(result);
        }
        catch (AggregateException ae)
        {
            if (ae.InnerException is AuthenticationException e) 
                WriteException(e);
        }
    }

    private static void WriteMessage(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    private static void WriteException(Exception e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.Message);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    private static string[] SplitString(string str)
    {
        return ParseStringHelper.SplitStringWithQuotes(str);
    }
}