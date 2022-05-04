using System;
using System.Linq;
using ConsolesShop.User;

namespace ConsolesShop.Command;

public class LoginCommand : BasicCommand
{
    private static readonly string[] Names = { "li", "login" };
    private readonly IUser[] _users;
    private string _name;
    private string _password;

    public LoginCommand(IUser[] users) : base(Names)
    {
        _users = users;
    }

    public override bool CanExecute(IUser user, string[] args)
    {
        return user is Guest && TryParseLoginAndPassword(args);
    }

    public override void Execute(string[] args)
    {
        if (!TryParseLoginAndPassword(args)) return;
            var user = TryFindUserByName();
            if (user is RegisteredUser us)
            {
                Console.WriteLine(us.Login(_password) ? "Login successful" : "Password incorrect");
            }
    }

    private IUser TryFindUserByName()
    {
        return _users.FirstOrDefault(user => user.Name == _name);
    }
    private bool TryParseLoginAndPassword(string[] args)
    {
        if (args is null)
            return false;
        try
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-n")
                    _name = args[i + 1];
                if (args[i] == "-p")
                    _password = args[i + 1];
            }

            return true;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }
    }

    public override void GetHelp()
    {
        Console.WriteLine("Login \t Login or li \t -n -p");
    }
}