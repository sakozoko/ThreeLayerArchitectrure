using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Objects;

namespace MarketUI.Command;

public class OrderHistoryViewCommand : BaseCommand
{
    private static readonly string[] Parameters = { "-u" };
    private readonly IServiceContainer _serviceContainer;
    private int _id;

    public OrderHistoryViewCommand(IServiceContainer serviceContainer) : base(Parameters)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("ID \t Description \t Status");
        Task<IEnumerable<Order>> res;
        if (TryParseId(args))
        {
            var taskUser = _serviceContainer.UserService.GetById(ConsoleUserInterface.AuthenticationData?.Token, _id);
            res = _serviceContainer.OrderService.GetUserOrders(ConsoleUserInterface.AuthenticationData?.Token,
                taskUser.Result);
        }
        else
        {
            res = _serviceContainer.OrderService.GetUserOrders(ConsoleUserInterface.AuthenticationData?.Token);
        }

        foreach (var order in res.Result)
        {
            stringBuilder.Append($"\n {order.Id} \t {order.Description} \t {order.OrderStatus}");
            stringBuilder.Append("\n\tProducts:");
            foreach (var orderProduct in order.Products)
                stringBuilder.Append($"\n {orderProduct.Name} \t {orderProduct.Cost} \t {orderProduct.Category.Name}");
            stringBuilder.Append("\n-------------------------------------------------------");
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