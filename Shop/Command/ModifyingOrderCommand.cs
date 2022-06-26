using System.Collections.Generic;
using BLL;
using BLL.Services.Factory;
using Entities;

namespace Shop.Command;

public class ModifyingOrderCommand : BaseCommand
{
    private static readonly string[] Names = { "mo", "modifyorder" };
    private static readonly string[] Parameters = { "-o", "-p", "-a", "-r", "-d", "-s" };
    private readonly IServiceContainer _serviceContainer;
    private Dictionary<string, string> _dict;

    public ModifyingOrderCommand(IServiceContainer serviceContainer) : base(Names, Parameters)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        if (!ArgumentsContainsOrderId(args)) return GetHelp();

        _dict.TryGetValue(Parameters[0], out var stringOrderId);

        if (!int.TryParse(stringOrderId, out var orderId)) return GetHelp();

        var ord = _serviceContainer.OrderService.GetById(ConsoleUserInterface.AuthenticationData.Token, orderId);

        if (ord.Result is null) return "Order not found";

        ModifyOrderProducts(ord.Result);
        ModifyDesc(ord.Result);
        ModifyOrderStatus(ord.Result);

        return $"Order with id {orderId} updated";
    }

    private void ModifyOrderStatus(Order ord)
    {
        OrderStatus? orderStatus = null;


        if (_dict.ContainsKey(Parameters[5])) orderStatus = GetOrderStatusFromArgumentsDictionary();

        if (orderStatus.HasValue)
            _serviceContainer.OrderService.ChangeOrderStatus(ConsoleUserInterface.AuthenticationData.Token, orderStatus.Value, ord);
    }

    private void ModifyDesc(Order ord)
    {
        _dict.TryGetValue(Parameters[4], out var desc);
        if (string.IsNullOrWhiteSpace(desc)) return;
        _serviceContainer.OrderService.ChangeDescription(ConsoleUserInterface.AuthenticationData.Token, desc, ord);
    }

    public override string GetHelp()
    {
        return "Modyfing order \t mo or modifyorder \t -o, -p, -a, -r, -d, -s";
    }

    private void ModifyOrderProducts(Order ord)
    {
        _dict.TryGetValue(Parameters[1], out var stringProductId);

        if (!int.TryParse(stringProductId, out var productId)) return;
        if (productId <= 0) return;

        var product = _serviceContainer.ProductService.GetById(ConsoleUserInterface.AuthenticationData.Token, productId);
        bool? addProduct = null;

        if (_dict.ContainsKey(Parameters[2])) addProduct = true;

        if (_dict.ContainsKey(Parameters[3])) addProduct = false;

        if (!addProduct.HasValue || product.Result is null) return;
        if (addProduct.Value)
            _serviceContainer.OrderService.AddProduct(ConsoleUserInterface.AuthenticationData.Token, product.Result,
                ord);
        else
            _serviceContainer.OrderService.DeleteProduct(ConsoleUserInterface.AuthenticationData.Token, product.Result,
                ord);
    }

    private OrderStatus? GetOrderStatusFromArgumentsDictionary()
    {
        var status = _dict[Parameters[5]];
        return status switch
        {
            nameof(OrderStatus.New) => OrderStatus.New,
            nameof(OrderStatus.CanceledByTheAdministrator) => OrderStatus.CanceledByTheAdministrator,
            nameof(OrderStatus.PaymentReceived) => OrderStatus.PaymentReceived,
            nameof(OrderStatus.Sent) => OrderStatus.Sent,
            nameof(OrderStatus.Completed) => OrderStatus.Completed,
            nameof(OrderStatus.Received) => OrderStatus.Received,
            nameof(OrderStatus.CanceledByUser) => OrderStatus.CanceledByTheAdministrator,
            _ => null
        };
    }

    private bool ArgumentsContainsOrderId(string[] args)
    {
        return TryParseArgs(args, out _dict) && _dict.ContainsKey(Parameters[0]);
    }
}