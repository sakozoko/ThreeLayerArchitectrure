using MarketUI.Util.Interface;

namespace MarketUI.Command;

public class LogoutCommand : BaseCommand
{
    public LogoutCommand(IUserInterfaceMapperHandler mapperHandler, ICommandsInfoHandler cih) : base(mapperHandler, cih)
    {
    }

    public override string Execute(string[] args)
    {
        if (ConsoleUserInterface.AuthenticationData is null) return "Error";
        var str = $"{ConsoleUserInterface.AuthenticationData.Name}, bye bye!";
        ConsoleUserInterface.AuthenticationData = null;
        return str;
    }
}