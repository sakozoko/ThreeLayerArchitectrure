using System;
using Autofac;
using MarketUI.Extension;
using MarketUI.Util;

namespace MarketUI;

public static class Program
{
    private static ConsoleUserInterface _consoleUserInterface;

    private static void Main(string[] args)
    {
        var cihJson = new CommandsInfoHandlerJson();
        var container = AppConfiguration.Configure(cihJson);
        _consoleUserInterface = container.Resolve<ConsoleUserInterface>();
        Start(args);
    }

    private static void Start(string[] args)
    {
        var k = args.ConcatWithSeparator(" ") ?? Console.ReadLine();

        while (k != "exit")
        {
            _consoleUserInterface.ExecuteCommand(k);
            k = Console.ReadLine();
        }
    }
}