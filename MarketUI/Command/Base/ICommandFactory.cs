namespace MarketUI.Command.Base;

public interface ICommandFactory
{
    public IExecutableCommand GetCommand(string name);
}