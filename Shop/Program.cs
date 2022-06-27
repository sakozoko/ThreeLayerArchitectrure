using System;

namespace Shop;

public static class Program
{
    private static readonly ConsoleUserInterface ConsoleUserInterface = new(new Command.CommandFactory());

    private static void Main(string[] args)
    {
        var k = Console.ReadLine();
        while (k != "exit")
        {
            try
            {
                ConsoleUserInterface.ExecuteCommand(k);
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception!!!");
            }

            k = Console.ReadLine();
        }
    }
}