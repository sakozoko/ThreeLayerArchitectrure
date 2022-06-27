using System;
using System.Collections.Generic;

namespace Shop.Command;

public abstract class BaseCommand : ICommand
{
    public abstract string[] Names { get; }
    private readonly string[] _params;

    protected BaseCommand(string[] parameters = null)
    {
        _params = parameters;
    }

    public abstract string Execute(string[] args);
    public abstract string GetHelp();

    /// <summary>
    ///     Args must be as "-key, value, -key, value, etc",then algorithm working
    /// </summary>
    /// <param name="args">Args</param>
    /// <returns></returns>
    private Dictionary<string, string> ParseArgs(string[] args)
    {
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