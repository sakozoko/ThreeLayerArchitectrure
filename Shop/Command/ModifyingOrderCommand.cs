using System.Collections.Generic;
using BLL;
using Entities;

namespace Shop.Command;

public class ModifyingOrderCommand : BaseCommand
{
    private static readonly string[] Names = { "mo", "modifyorder" };
    private static readonly string[] Parameters = { "-o", "-p", "-a", "-r", "-d", "-s" };
    private readonly Service _service;
    private Dictionary<string, string> _dict;

    public ModifyingOrderCommand(Service service) : base(Names, Parameters)
    {
        _service = service;
    }

    public override string Execute(string[] args)
    {
        if (!ArgumentsContainsOrderId(args)) return GetHelp();

        _dict.TryGetValue(Parameters[0], out var stringOrderId);

        if (!int.TryParse(stringOrderId, out var orderId)) return GetHelp();

        var ord = _service.Factory.OrderService.GetById(Shop.AuthenticationData.Token, orderId);

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
            _service.Factory.OrderService.ChangeOrderStatus(Shop.AuthenticationData.Token, orderStatus.Value, ord);
    }

    private void ModifyDesc(Order ord)
    {
        _dict.TryGetValue(Parameters[4], out var desc);
        if (string.IsNullOrWhiteSpace(desc)) return;
        _service.Factory.OrderService.ChangeDescription(Shop.AuthenticationData.Token, desc, ord);
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

        var product = _service.Factory.ProductService.GetById(Shop.AuthenticationData.Token, productId);
        bool? addProduct = null;

        if (_dict.ContainsKey(Parameters[2])) addProduct = true;

        if (_dict.ContainsKey(Parameters[3])) addProduct = false;

        if (!addProduct.HasValue || product.Result is null) return;
        if (addProduct.Value)
            _service.Factory.OrderService.AddProduct(Shop.AuthenticationData.Token, product.Result,
                ord);
        else
            _service.Factory.OrderService.DeleteProduct(Shop.AuthenticationData.Token, product.Result,
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