using System;
using BLL.Services.Exception;
using MarketUI.Command;
using MarketUI.Extension;
using MarketUI.Models;

namespace MarketUI;

public class ConsoleUserInterface
{
    private readonly ICommandFactory _factory;

    public ConsoleUserInterface(ICommandFactory factory)
    {
        _factory = factory;
    }

    public static AuthenticateResponseModel AuthenticationData { get; set; }


    public void ExecuteCommand(string commandString)
    {
        var args = commandString.SplitStringWithQuotes();
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
}