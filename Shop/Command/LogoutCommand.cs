namespace Shop.Command;

public class LogoutCommand : BasicCommand
{
    private static readonly string[] Names = { "logout", "lo" };

    public LogoutCommand() : base(Names)
    {
    }


    public override string Execute(string[] args)
    {
        if (Shop.AuthenticationData is null) return "Error";
        var str = $"{Shop.AuthenticationData.Name}, bye bye!";
        Shop.AuthenticationData = null;
        return str;
    }

    public override string GetHelp()
    {
        return "Logout \t logout or lo \t none";
    }
}