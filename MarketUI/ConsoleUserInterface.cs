using System;
using BLL.Util.Services.Exception;
using Entities;
using MarketUI.Command;
using MarketUI.Helpers;

namespace MarketUI;

public class ConsoleUserInterface
{
    private readonly ICommandFactory _factory;

    public ConsoleUserInterface(ICommandFactory factory)
    {
        _factory = factory;
    }

    public static AuthenticateResponse AuthenticationData { get; set; }

    public void ExecuteCommand(string commandString)
    {
        var args = SplitString(commandString);
        try
        {
            var result = _factory.GetCommand(args[0]).Execute(args);
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