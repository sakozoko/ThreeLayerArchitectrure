using Entities.User;

namespace BLL.Command;

public class IncorrectCommand : BasicCommand
{
    public IncorrectCommand() : base(Array.Empty<string>())
    {
    }

    public override bool CanExecute(IUser user, string[] args = null)
    {
        return true;
    }

    public override Task<string> Execute(string[] args = null)
    {
        return Task<string>.Factory.StartNew(() => "Write h or help to help about commands");
    }

    public override Task<string>  GetHelp()
    {
        return Task<string>.Factory.StartNew(() => "Incorrect \t none \t none");
    }
}