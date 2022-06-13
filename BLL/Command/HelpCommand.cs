using System.Text;
using Entities.User;

namespace BLL.Command;

public class HelpCommand : BasicCommand
{
    private static readonly string[] Names = { "h", "help" };
    private readonly ICommand[] _commands;

    public HelpCommand(ICommand[] commands) : base(Names)
    {
        _commands = commands;
    }

    public override bool CanExecute(IUser user, string[] args = null)
    {
        return true;
    }

    public override Task<string> Execute(string[] args=null)
    {
        return Task<string>.Factory.StartNew(() =>
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Name \t To invoke \t Args");
            foreach (var command in _commands)
            {
                var str = command.GetHelp().Result;
                stringBuilder.Append("\n" + str);
            }

            stringBuilder.Append(GetHelp().Result);
            return stringBuilder.ToString();
        });

    }

    public override Task<string>GetHelp()
    {
        return Task<string>.Factory.StartNew(() => "Help \t h or help \t none");
    }
}