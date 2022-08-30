using System;
using BLL.Services.Exception;
using MarketUI.Command.Base;
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
        catch (AuthenticationException ae)
        {
            WriteException(ae);
        }
        catch (AggregateException ae)
        {
            WriteException(ae.InnerException);
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