using System;
using System.Collections.Generic;
using ConsolesShop.Goods;

namespace ConsolesShop.Command;

public class ViewProductsCommand : BasicCommand
{
    private static readonly string[] Names = { "view", "v", "vp" };
    private readonly List<Product> _products;

    public ViewProductsCommand(List<Product> products) : base(Names)
    {
        _products = products;
    }


    public override void Execute(string[] args)
    {
        Console.WriteLine("# Name \t Product category \t Product cost \t Product description");
        foreach (var product in _products)
            Console.WriteLine($"{product.Id} " +
                              $"{product.Name} \t " +
                              $"{product.Category} \t " +
                              $"{product.Cost} \t " +
                              $"{product.Description}");
    }

    public override void GetHelp()
    {
        Console.WriteLine("View products \t view, v, or vp \t none");
    }
}