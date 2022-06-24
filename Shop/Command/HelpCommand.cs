using System.Text;

namespace Shop.Command;

public class HelpCommand : BasicCommand
{
    private static readonly string[] Names = { "h", "help" };
    private readonly ICommand[] _commands;

    public HelpCommand(ICommand[] commands) : base(Names)
    {
        _commands = commands;
    }

    public override string Execute(string[] args)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Name \t To invoke \t Args");
        foreach (var command in _commands)
        {
            var str = command.GetHelp();
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