using System;
using ConsolesShop.User;

namespace ConsolesShop.Command;

public class IncorrectCommand : BasicCommand
{
    public IncorrectCommand() : base(Array.Empty<string>())
    {
    }

    public override bool CanExecute(IUser user, string[] args)
    {
        return true;
    }

    public override void Execute(string[] args)
    {
        Console.WriteLine("Write h or help to help about commands");
    }

    public override void GetHelp()
    {
        Console.WriteLine("Incorrect \t none \t none");
    }
}