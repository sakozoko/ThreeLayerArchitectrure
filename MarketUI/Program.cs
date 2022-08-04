
using System;
using System.Diagnostics;
using System.Text;
using Autofac;
using MarketUI.Util;

namespace MarketUI;

public static class Program
{
    private static ConsoleUserInterface _consoleUserInterface;

    private static void Main(string[] args)
    {
        var container = AppConfiguration.Configure();
        _consoleUserInterface = container.Resolve<ConsoleUserInterface>();
        Start(args);
    }
    private static string ConcatWithSeparator(this string[] args, string separator)
    {
        var stringBuilder = new StringBuilder();
        foreach (var arg in args)
        {
            stringBuilder.Append(arg);
            stringBuilder.Append(separator);
        }

        return stringBuilder.ToString();
    }
    private static void Start(string[] args)
    {
        var k = args.ConcatWithSeparator(" ") ?? Console.ReadLine();

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