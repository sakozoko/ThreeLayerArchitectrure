using System.Linq;
using Autofac;
using ConsoleTable;
using MarketUI.Command.Base;
using MarketUI.Extension;

namespace MarketUI.Command.Util;

public class HelpCommand : IExecutableCommand
{
    private readonly IContainer _container;

    public HelpCommand(IContainer container)
    {
        _container = container;
    }

    public string Execute(string[] args)
    {
        var commands = _container.ResolveAll<IDescriptiveCommand>();
        var consoleTable = new Table()
            .AddColumn("Name", "To invoke", "Args")
            .AddAlignment(Alignment.Center);
        foreach (var command in commands)
        {
                var str =command.GetHelp();
                consoleTable.AddRow(str.Split("\t").ToArray<object>());
        }

        consoleTable.AddRow("Help", "h or help", "none");
        return consoleTable.ToString();
    }
}