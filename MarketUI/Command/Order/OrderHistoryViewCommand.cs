using System.Collections.Generic;
using System.Linq;
using BLL;
using MarketUI.Command.Base;
using MarketUI.Models;
using MarketUI.Util;
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
            .GetUserOrders(ConsoleUserInterface.AuthenticationData?.Token, _id).Result);

        var consoleTable = new ConsoleTable();
        consoleTable.AddColumn("#", "Description", "Status");

        foreach (var order in orderModels)
        {
            consoleTable.AddRow(order.Id, order.Description, order.OrderStatus);
            var consoleTable2 = new ConsoleTable(consoleTable.GetCalculatedPadding());
            if (order.Products.Count == 0)
                continue;

            consoleTable2.AddColumn("Name", "Count", "Sum");

            foreach (var orderProductGroup in order.Products.GroupBy(x => x.Id))
                consoleTable2.AddRow(orderProductGroup.First().Name, orderProductGroup.Count(),
                    orderProductGroup.Sum(x => x.Cost));
            consoleTable.AddRowWithoutColumn(new string(' ', 15) + "Products:\n" + consoleTable2 + "\n" +
                                             new string(' ', 15) + "Total: " + order.Total);
        }


        return consoleTable.ToString();
    }
}