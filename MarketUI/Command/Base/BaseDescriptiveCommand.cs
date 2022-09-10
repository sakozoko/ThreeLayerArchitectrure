using MarketUI.Util.Interface;

namespace MarketUI.Command.Base
{
    public abstract class BaseDescriptiveCommand : IDescriptiveCommand
    {
        protected readonly ICommandsInfoHandler CommandsInfo;

        protected BaseDescriptiveCommand(ICommandsInfoHandler commandsInfo)
        {
            CommandsInfo = commandsInfo;
        }

        public string GetHelp()
        {
            return CommandsInfo.GetCommandInfo(GetType().Name).Tip;
        }
    }
}