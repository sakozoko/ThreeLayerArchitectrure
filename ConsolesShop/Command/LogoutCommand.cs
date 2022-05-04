using System;
using ConsolesShop.User;

namespace ConsolesShop.Command;

public class LogoutCommand : BasicCommand
{
    private static readonly string[] Names = { "logout", "lo" };

    public LogoutCommand() : base(Names)
    {
    }

    public override bool CanExecute(IUser user, string[] args)
    {
        if (user is not RegisteredUser) return false;
        _user = user;
        return true;
    }

    public override void Execute(string[] args)
    {
        (_user as RegisteredUser).Logout();
    }

    public override void GetHelp()
    {
        Console.WriteLine("Logout \t logout or lo \t none");
    }
}