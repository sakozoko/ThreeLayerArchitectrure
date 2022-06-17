﻿using System;
using System.Threading.Tasks;

namespace Shop.Command;

public class IncorrectCommand : BasicCommand
{
    public IncorrectCommand() : base(Array.Empty<string>())
    {
    }

    public override string Execute(string[] args)
    {
        return "Write h or help to help about commands";
    }

    public override string GetHelp()
    {
        return "Incorrect \t none \t none";
    }
}