using MarketUI.Models;

namespace MarketUI.Util.Interface;

public interface ICommandsInfoHandler
{
    CommandInfoModel GetCommandInfo(string commandName);
}