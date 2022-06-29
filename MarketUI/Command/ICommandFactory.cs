namespace MarketUI.Command;

public interface ICommandFactory
{
    public ICommand GetCommand(string name);
}