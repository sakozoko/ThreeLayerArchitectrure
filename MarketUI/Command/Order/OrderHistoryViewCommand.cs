using System.Collections.Generic;
using System.Linq;
using BLL;
using ConsoleTable;
using MarketUI.Command.Base;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command.Order;

public class OrderHistoryViewCommand : BaseCommand
{
    private readonly IServiceContainer _serviceContainer;
    private int _id;

    public OrderHistoryViewCommand(IServiceContainer serviceContainer, IUserInterfaceMapperHandler mapperHandler,
        ICommandsInfoHandler cih) :
        base(mapperHandler, cih)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        if (TryParseArgs(args, out var dict))
        {
            dict.TryGetValue(Parameters[0], out var id);
            int.TryParse(id, out _id);
        }

        var orderModels = Mapper.Map<IEnumerable<OrderModel>>(_serviceContainer.OrderService
            .GetUserOrders(ConsoleUserInterface.AuthenticationData?.Token, _id).Result).ToArray();

        var consoleTable = new Table()
            .AddColumn("#", "Description", "Status")
            .AddAlignment(Alignment.Center, 1)
            .AddAlignment(Alignment.Center, 2);

        SeedRows(consoleTable, orderModels.Where(x=>x.Confirmed));

        consoleTable.AddRowWithoutColumn("Unconfirmed orders:");
        SeedRows(consoleTable, orderModels.Where(x=>!x.Confirmed));

        return consoleTable.ToString();
    }

    private static void SeedRows(Table consoleTable, IEnumerable<OrderModel> orderModels)
    {
        foreach (var order in orderModels)
        {
            consoleTable.AddRow(order.Id, order.Description, order.OrderStatus);
            var consoleTable2 = new Table()
                .AddColumn("Name", "Count", "Sum")
                .AddCustomFormat(typeof(decimal), "{0:0.00}")
                .AddAlignment(Alignment.Center)
                .AddAlignment(Alignment.Left, 0);
            if (order.Products.Count == 0)
                continue;
            consoleTable.AddSeparatorForEachRow();
            foreach (var orderProductGroup in order.Products.GroupBy(x => x.Id))
                consoleTable2.AddRow(orderProductGroup.First().Name, orderProductGroup.Count(),
                    orderProductGroup.Sum(x => x.Cost));
            consoleTable.AddRowWithoutColumn(new string(' ', 15) + "Products:")
                .AddTable(consoleTable2)
                .AddRowWithoutColumn(new string(' ', 15) + "Total: " + order.Total, RowOrder.After);
        }
    }
}