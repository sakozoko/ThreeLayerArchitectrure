using System;
using ConsolesShop.User;

namespace ConsolesShop.Command;

public class ViewOrderHistoryCommand : BasicCommand
{
    private static readonly string[] Names = { "view orders", "vo" };
    public ViewOrderHistoryCommand() : base(Names)
    {
    }

    public override bool CanExecute(IUser user, string[] args)
    {
        _user ??= user;
        return user is RegisteredUser;
    }

    public override void Execute(string[] args)
    {
        Console.WriteLine("ID \t Description \t Is cancelled \t Is delivered");
        foreach (var order in (_user as RegisteredUser).Orders)
        {
            Console.WriteLine(order);
        }
    }

    public override void GetHelp()
    {
        Console.WriteLine("View order history \t show orders, so \t none");
    }
}