namespace MarketUI.Command;

public class LogoutCommand : BaseCommand
{
    public override string Execute(string[] args)
    {
        if (ConsoleUserInterface.AuthenticationData is null) return "Error";
        var str = $"{ConsoleUserInterface.AuthenticationData.Name}, bye bye!";
        ConsoleUserInterface.AuthenticationData = null;
        return str;
    }

    public override string GetHelp()
    {
        return "Logout \t logout or lo \t none";
    }
}