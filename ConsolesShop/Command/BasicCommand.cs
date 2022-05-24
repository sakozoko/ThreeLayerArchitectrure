using System;
using System.Collections.Generic;
using System.Linq;
using ConsolesShop.User;

namespace ConsolesShop.Command;

public abstract class BasicCommand : ICommand
{
    private readonly string[] _names;
    private readonly string[] _params;
    protected IUser _user;

    protected BasicCommand(string[] names, string[] parameters=null)
    {
        _names = names;
        _params = parameters;
    }

    public bool ItsMe(string commandName)
    {
        if (_names is not null)
            return _names.Any(name =>
                commandName.Equals(name,
                    StringComparison.OrdinalIgnoreCase));
        return false;
    }
    
    /// <summary>
    /// Args must be as "-key, value, -key, value, etc",then algorithm worked
    /// </summary>
    /// <param name="args">Args</param>
    /// <returns></returns>
    protected virtual Dictionary<string,string> ParseArgs(string[] args)
    {
        if (_params == null)
            throw new NullReferenceException("Params is null");
        if (args == null)
            throw new ArgumentNullException(nameof(args));
        
        var dict = new Dictionary<string, string>();
        
        for(var i=0;i<args.Length;i++)
        {
            var isParsed = false;
            
            foreach (var key in _params)
            {
                if (args[i] != key) continue;
                
                dict[key] = args[i + 1];
                isParsed = true;
            }

            if (isParsed)
            {
                i++;
            }
        }

        return dict;
    }

    public virtual bool CanExecute(IUser user, string[] args)
    {
        _user ??= user;
        return user is Administrator;
    }

    public abstract void Execute(string[] args);
    
    
    public abstract void GetHelp();
}