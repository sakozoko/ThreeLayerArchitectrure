using System.Text;
using Autofac;
using MarketUI.Extension;

namespace MarketUI.Command;

public class HelpCommand :ICommand
{
    private readonly IContainer _container;

    public HelpCommand(IContainer container)
    {
        _container = container;
    }

    public string Execute(string[] args)
    {
        var commands = _container.ResolveAll<ICommand>();
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Name \t To invoke \t Args");
        foreach (var command in commands)
        {
            var str = (command as BaseCommand)?.GetHelp();
            stringBuilder.Append("\n" + str);
        }

        stringBuilder.Append("Help \t h or help \t none");
        return stringBuilder.ToString();
    }
}