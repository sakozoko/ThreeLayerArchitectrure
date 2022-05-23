using System;
using ConsolesShop.User;

namespace ConsolesShop.Command;

public class LogoutCommand : BasicCommand
{
    private static readonly string[] Names = { "logout", "lo" };
    private readonly Action _saveSession;

    public LogoutCommand(Action act) : base(Names)
    {
        _saveSession = act;
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
        _saveSession();
    }

    public override void GetHelp()
    {
        Console.WriteLine("Logout \t logout or lo \t none");
    }
}