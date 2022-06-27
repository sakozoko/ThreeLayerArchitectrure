namespace MarketUI.Util.Command;

public interface ICommandFactory
{
    public ICommand GetCommand(string name);
}