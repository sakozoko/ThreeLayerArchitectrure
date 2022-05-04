using System;
using ConsolesShop.User;

namespace ConsolesShop.Command;

public class HelpCommand : BasicCommand
{
    private static readonly string[] Names = { "h", "help" };
    private readonly ICommand[] _commands;

    public HelpCommand(ICommand[] commands) : base(Names)
    {
        _commands = commands;
    }

    public override bool CanExecute(IUser user, string[] args)
    {
        return true;
    }

    public override void Execute(string[] args = null)
    {
        Console.WriteLine("Name \t To invoke \t Args");
        foreach (var command in _commands) command.GetHelp();
        GetHelp();
    }

    public override void GetHelp()
    {
        Console.WriteLine("Help \t h or help \t none");
    }
}