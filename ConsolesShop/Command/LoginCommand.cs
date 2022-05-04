using System;
using ConsolesShop.User;

namespace ConsolesShop.Command;

public class LoginCommand : BasicCommand
{
    private static readonly string[] Names = { "li", "login" };
    private readonly IUser[] _users;

    public LoginCommand(IUser[] users) : base(Names)
    {
        _users = users;
    }

    public override bool CanExecute(IUser user, string[] args)
    {
        return user is Guest;
    }

    public override void Execute(string[] args)
    {
        (_users[0] as RegisteredUser).Login("123123");
    }

    public override void GetHelp()
    {
        Console.WriteLine("Login \t Login or li \t -a -p");
    }
}