using System.Threading.Tasks;

namespace Shop.Command;

public class LogoutCommand : BasicCommand
{
    private static readonly string[] Names = { "logout", "lo" };

    public LogoutCommand() : base(Names)
    {
    }


    public override Task<string> Execute(string[] args)
    {
        return Task<string>.Factory.StartNew(() =>
        {
            if (Shop.AuthenticationData is null) return "Error";

            var str = $"{Shop.AuthenticationData.Name}, bye bye!";
            Shop.AuthenticationData = null;
            return str;
        });
    }

    public override string GetHelp()
    {
        return "Logout \t logout or lo \t none";
    }
}