using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command;

public class OrderHistoryViewCommand : BaseCommand
{
    private static readonly string[] Parameters = { "-u" };
    private readonly IServiceContainer _serviceContainer;
    private int _id;

    public OrderHistoryViewCommand(IServiceContainer serviceContainer, IUserInterfaceMapperHandler mapperHandler) : 
        base(mapperHandler,Parameters)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("ID \t Description \t Status");
        IEnumerable<OrderModel> res;
        if (TryParseId(args))
        {
            var taskUser = _serviceContainer.UserService.GetById(ConsoleUserInterface.AuthenticationData?.Token, _id);
            res = Mapper.Map<IEnumerable<OrderModel>>(_serviceContainer.OrderService.GetUserOrders(ConsoleUserInterface.AuthenticationData?.Token,
                taskUser.Result).Result);
        }
        else
        {
            res = Mapper.Map<IEnumerable<OrderModel>>(_serviceContainer.OrderService.GetUserOrders(ConsoleUserInterface.AuthenticationData?.Token).Result);
        }

        foreach (var order in res)
        {
            stringBuilder.Append($"\n {order.Id} \t {order.Description} \t {order.OrderStatus}");
            stringBuilder.Append("\n\tProducts:");
            
            foreach (var orderProductGroup in order.Products.GroupBy(x => x.Id))
            {
                stringBuilder.Append($"\n {orderProductGroup.First().Name} \t  {orderProductGroup.Count()}\t  {orderProductGroup.Sum(x=>x.Cost)}");
            }

            stringBuilder.Append("\n-------------------------------------------------------");
            stringBuilder.Append("\n\tTotal: " + order.Total);
        }


        return stringBuilder.ToString();
    }

    private bool TryParseId(string[] args)
    {
        if (TryParseArgs(args, out var dict))
            return dict.TryGetValue(Parameters[0], out var id) && int.TryParse(id, out _id);
        return false;
    }

    public override string GetHelp()
    {
        return "View order history \t view orders, vo \t -u";
    }
}