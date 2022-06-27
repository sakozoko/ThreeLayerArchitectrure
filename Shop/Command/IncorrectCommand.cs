using System;

namespace Shop.Command;

public class IncorrectCommand : ICommand
{
    public IncorrectCommand() 
    {
    }


    public string[] Names { get; }

    public string Execute(string[] args)
    {
        return "Write h or help to help about commands";
    }
    
}