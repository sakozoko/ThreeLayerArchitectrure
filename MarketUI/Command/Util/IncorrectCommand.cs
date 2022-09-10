using MarketUI.Command.Base;

namespace MarketUI.Command.Util
{
    public class IncorrectCommand : IExecutableCommand
    {
        public string Execute(string[] args)
        {
            return "Write h or help to help about commands";
        }
    }
}