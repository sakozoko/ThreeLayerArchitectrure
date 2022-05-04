using System;
using System.Linq;
using ConsolesShop.User;

namespace ConsolesShop.Command;

public abstract class BasicCommand : ICommand
{
    private readonly string[] _names;
    protected IUser _user;

    protected BasicCommand(string[] names)
    {
        _names = names;
    }

    public bool ItsMe(string commandName)
    {
        if (_names is not null)
            return _names.Any(name =>
                commandName.Equals(name,
                    StringComparison.OrdinalIgnoreCase));
        return false;
    }

    public virtual bool CanExecute(IUser user, string[] args)
    {
        _user ??= user;
        return user is Administrator;
    }

    public abstract void Execute(string[] args);
    public abstract void GetHelp();
}