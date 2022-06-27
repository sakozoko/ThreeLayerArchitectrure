using System;
using BLL.Util.Services.Exception;
using Entities;
using MarketUI.Util.Command;
using MarketUI.Util.Helpers;

namespace MarketUI.Util;

public class ConsoleUserInterface
{
    private readonly ICommandFactory _commandFactory;

    public ConsoleUserInterface(ICommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }

    public static AuthenticateResponse AuthenticationData { get; set; }

    public void ExecuteCommand(string commandString)
    {
        var args = SplitString(commandString);
        var command = _commandFactory.GetCommand(args[0]);
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