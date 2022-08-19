namespace MarketUI.Command.Base;

public interface ICommand
{
    public string Execute(string[] args);
}