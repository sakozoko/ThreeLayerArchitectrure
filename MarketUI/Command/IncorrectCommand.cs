namespace MarketUI.Command;

public class IncorrectCommand : ICommand
{
    public string Execute(string[] args)
    {
        return "Write h or help to help about commands";
    }
}