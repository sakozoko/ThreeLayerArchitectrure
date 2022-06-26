﻿using System.Collections.Generic;
using System.Text;

namespace Shop.Command;

public class HelpCommand : BaseCommand
{
    private static readonly string[] Names = { "h", "help" };
    private readonly IEnumerable<ICommand> _commands;

    public HelpCommand(IEnumerable<ICommand> commands) : base(Names)
    {
        _commands = commands;
    }

    public override string Execute(string[] args)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Name \t To invoke \t Args");
        foreach (var command in _commands)
        {
            var str = (command as BaseCommand)?.GetHelp();
            stringBuilder.Append("\n" + str);
        }

        stringBuilder.Append(GetHelp());
        return stringBuilder.ToString();
    }

    public override string GetHelp()
    {
        return "Help \t h or help \t none";
    }
}