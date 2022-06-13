using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Shop
{
    
    public class Program
    {
  
        
        private static readonly BLL.Shop Shop=new BLL.Shop();
        static async Task TaskAsync1()
        {
            await Task.Factory.StartNew(() => Console.WriteLine("TaskAsync1 is working"));
        }
        static Task Task1()
        {
            return new Task(() => { Console.WriteLine("Task1 is working"); });
        }

        static Task<string> TaskResultsString()
        {
            return new Task<string>(() => "123");
        }
        static async Task Main(string[] args)
        {
            //Task1().Start();
            //await TaskAsync1();
            //////
            //Console.ReadKey();
                string k = Console.ReadLine();
                while (k != "exit")
                {
                    Shop.ExecuteCommand(k);
                    k = Console.ReadLine();
                }
        }
        
    }
}
