using System;
using System.Collections.Generic;


namespace Shop
{
    class Program
    {
        private static readonly ConsolesShop.Shop Shop=new ConsolesShop.Shop();
        static void Main(string[] args)
        {
            string k = Console.ReadLine();
            while (k != "exit")
            {
                Shop.ExecuteCommand(k);
                k = Console.ReadLine();
            }

        }
        
    }
}
