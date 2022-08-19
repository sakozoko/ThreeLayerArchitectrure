namespace MarketUI.Command.Base;

public interface ICommandFactory
{
    public ICommand GetCommand(string name);
}