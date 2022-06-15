using System;
using System.Threading.Tasks;

namespace Shop;

public static class Program
{
    private static readonly Shop Shop = new();

    private static async Task Main(string[] args)
    {
        var k = Console.ReadLine();
        while (k != "exit")
        {
            try
            {
                await Shop.ExecuteCommand(k);
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception!!!");
            }

            k = Console.ReadLine();
        }
    }
}