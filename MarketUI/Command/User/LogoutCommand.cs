using MarketUI.Command.Base;
using MarketUI.Util.Interface;

namespace MarketUI.Command.User;

public class LogoutCommand : BaseDescriptiveCommand, IExecutableCommand
{
    public LogoutCommand(ICommandsInfoHandler cih) : base(cih)
    {
    }

    public string Execute(string[] args)
    {
        if (ConsoleUserInterface.AuthenticationData is null) return "You cannot log out until you are logged in.";
        var str = $"{ConsoleUserInterface.AuthenticationData.Name}, bye bye!";
        ConsoleUserInterface.AuthenticationData = null;
        return str;
    }
}