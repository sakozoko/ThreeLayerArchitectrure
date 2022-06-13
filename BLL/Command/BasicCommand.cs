﻿using Entities.User;

namespace BLL.Command;

public abstract class BasicCommand : ICommand
{
    private readonly string[] _names;
    private readonly string[] _params;
    protected IUser User;

    protected BasicCommand(string[] names, string[] parameters = null)
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

    public virtual bool CanExecute(IUser user, string[] args = null)
    {
        User ??= user;
        return user is Administrator;
    }

    public abstract Task<string> Execute(string[] args = null);
    public abstract Task<string> GetHelp();

    /// <summary>
    ///     Args must be as "-key, value, -key, value, etc",then algorithm working
    /// </summary>
    /// <param name="args">Args</param>
    /// <returns></returns>
    protected Dictionary<string, string> ParseArgs(string[] args)
    {
        if (_params == null)
            throw new NullReferenceException("Params is null");
        if (args == null)
            throw new ArgumentNullException(nameof(args));

        var dict = new Dictionary<string, string>();

        for (var i = 0; i < args.Length; i++)
        {
            var isParsed = false;

            foreach (var key in _params)
            {
                if (!args[i].Equals(key)) continue;
                if (i + 1 < args.Length)
                    dict[key] = args[i + 1];
                else
                    dict[key] = "";
                isParsed = true;
            }

            if (isParsed) i++;
        }

        return dict;
    }

    protected bool TryParseArgs(string[] args, out Dictionary<string, string> dict)
    {
        dict = null;
        try
        {
            dict = ParseArgs(args);
        }
        catch (NullReferenceException)
        {
            return false;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }
        catch (ArgumentNullException)
        {
            return false;
        }

        return dict?.Count > 0;
    }
}