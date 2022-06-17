using System;

namespace Shop;

public static class Program
{
    private static readonly Shop Shop = new();
    
    private static void Main(string[] args)
    {
        var k = Console.ReadLine();
        while (k != "exit")
        {
            try
            { 
                Shop.ExecuteCommand(k);
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception!!!");
            }

            k = Console.ReadLine();
        }
    }
}