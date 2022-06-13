using Entities.User;

namespace BLL.Command;

public class LogoutCommand : BasicCommand
{
    private static readonly string[] Names = { "logout", "lo" };
    private readonly Action _saveSession;

    public LogoutCommand(Action act) : base(Names)
    {
        _saveSession = act;
    }

    public override bool CanExecute(IUser user, string[] args = null)
    {
        if (user is null) return false;
        if (user.IsLoggedIn == false) return false;
        User = user;
        return true;
    }

    public override Task<string> Execute(string[] args = null)
    {
        return Task<string>.Factory.StartNew(() =>
        {
            User.Logout();
            _saveSession();
            return $"{User.Name}, bye bye";
        });

    }

    public override Task<string>  GetHelp()
    {
        return Task<string>.Factory.StartNew(() =>"Logout \t logout or lo \t none");
    }
}