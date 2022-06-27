using System;
using Autofac;

namespace MarketUI.Util;

public static class Program
{
    private static ConsoleUserInterface _consoleUserInterface;

    private static void Main(string[] args)
    {
        var container = AppConfiguration.Configure();
        _consoleUserInterface = container.Resolve<ConsoleUserInterface>();
        Start();
    }

    private static void Start()
    {
        var k = Console.ReadLine();
        while (k != "exit")
        {
            try
            {
                _consoleUserInterface.ExecuteCommand(k);
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception!!!");
            }

            k = Console.ReadLine();
        }
    }
}