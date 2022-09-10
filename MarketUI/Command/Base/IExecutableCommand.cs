namespace MarketUI.Command.Base
{
    public interface IExecutableCommand
    {
        public string Execute(string[] args);
    }
}