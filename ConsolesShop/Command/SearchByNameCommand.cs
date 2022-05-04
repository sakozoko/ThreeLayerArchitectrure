using System;
using System.Collections.Generic;
using ConsolesShop.Goods;
using ConsolesShop.User;

namespace ConsolesShop.Command;

public class SearchByNameCommand : BasicCommand
{
    private static readonly string[] Names = { "findby", "f", "s" };
    private readonly List<Product> _products;

    public SearchByNameCommand(List<Product> products) : base(Names)
    {
        _products = products;
    }

    public override bool CanExecute(IUser user, string[] args)
    {
        return true;
    }

    public override void Execute(string[] args)
    {
        Console.WriteLine("SearchByName is fine");
    }

    public override void GetHelp()
    {
        Console.WriteLine("Search by name command \t findby, f, s \t none");
    }
}