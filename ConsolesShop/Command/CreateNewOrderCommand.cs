using System;
using ConsolesShop.User;

namespace ConsolesShop.Command;

public class CreateNewOrderCommand: BasicCommand
{
    private static readonly string[] Names = { "cno", "order","createno" };
    
    public CreateNewOrderCommand() : base(Names)
    {
    }

    public override bool CanExecute(IUser user, string[] args)
    {
        _user ??= user;
        return user is RegisteredUser;
    }

    public override void Execute(string[] args)
    {
        (_user as RegisteredUser).CreateNewOrder();
        Console.WriteLine("OK!");
    }

    public override void GetHelp()
    {
        Console.WriteLine("Create new order \t cno or order or createno \t none");
    }
}