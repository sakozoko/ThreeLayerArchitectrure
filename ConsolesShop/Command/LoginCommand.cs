using System;
using System.Collections.Generic;
using System.Linq;
using ConsolesShop.User;

namespace ConsolesShop.Command;

public class LoginCommand : BasicCommand
{
    private static readonly string[] Names = { "li", "login" };
    private static readonly string[] Parameters = {"-n", "-p" };
    private readonly Action<RegisteredUser> _saveSession;
    private readonly IUser[] _users;
    private string _name;
    private string _password;

    public LoginCommand(IUser[] users, Action<RegisteredUser> act) : base(Names, Parameters)
    {
        _users = users;
        _saveSession = act;
    }

    public override bool CanExecute(IUser user, string[] args)
    {
        return user is Guest && TryParseLoginAndPassword(args);
    }

    public override void Execute(string[] args)
    {
        if (!TryParseLoginAndPassword(args)) return;
        var user = TryFindUserByName();
        switch (user)
        {
            case null:
                Console.WriteLine("Name incorrect");
                break;
            case RegisteredUser us:
                Console.WriteLine(us.Login(_password) ? "Login successful" : "Password incorrect");
                _saveSession(us);
                break;
        }
    }

    private IUser TryFindUserByName()
    {
        return _users.FirstOrDefault(user => user.Name == _name);
    }
    private bool TryParseLoginAndPassword(string[] args)
    {
        try
        {
            var dict = ParseArgs(args);
            return dict.TryGetValue(Parameters[0], out _name)
                   && dict.TryGetValue(Parameters[1], out _password);
        }
        catch (NullReferenceException)
        {
            return false;
        }
        catch (ArgumentNullException)
        {
            return false;
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