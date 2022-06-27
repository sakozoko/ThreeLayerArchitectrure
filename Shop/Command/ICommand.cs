namespace Shop.Command;

public interface ICommand
{
    public string Execute(string[] args);
}