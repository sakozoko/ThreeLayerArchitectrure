namespace MarketUI.Util.Command;

public class IncorrectCommand : ICommand
{
    public string[] Names { get; }

    public string Execute(string[] args)
    {
        return "Write h or help to help about commands";
    }
}