using System.Collections.Generic;
using BLL;
using MarketUI.Command.Base;
using MarketUI.Util.Interface;

namespace MarketUI.Command.Order;

public class OrderModifyingCommand : BaseCommand
{
    private readonly IServiceContainer _serviceContainer;
    private Dictionary<string, string> _dict;

    public OrderModifyingCommand(IServiceContainer serviceContainer, IUserInterfaceMapperHandler mapperHandler,
        ICommandsInfoHandler cih) :
        base(mapperHandler, cih)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        if (!ArgumentsAreValid(args)) return GetHelp();

        _dict.TryGetValue(Parameters[0], out var stringOrderId);

        int.TryParse(stringOrderId, out var orderId);

        if (TryChangeOrderProducts(orderId) | TryChangeDescription(orderId) | TryChangeOrderStatus(orderId))
            return $"Order with id {orderId} updated";

        return "Something wrong";
    }

    private bool TryChangeOrderStatus(int orderId)
    {
        return _dict.TryGetValue(Parameters[4], out var stringOrderStatus) &&
               _serviceContainer.OrderService.ChangeOrderStatus(ConsoleUserInterface.AuthenticationData.Token,
                   stringOrderStatus,
                   orderId).Result;
    }

    private bool TryChangeDescription(int orderId)
    {
        return _dict.TryGetValue(Parameters[3], out var desc) && !string.IsNullOrWhiteSpace(desc) &&
               _serviceContainer.OrderService.ChangeDescription(ConsoleUserInterface.AuthenticationData.Token, desc,
                   orderId).Result;
    }

    private bool TryChangeOrderProducts(int orderId)
    {
        _dict.TryGetValue(Parameters[1], out var stringProductId);

        if (!int.TryParse(stringProductId, out var productId)) return false;

        var addProduct = !_dict.ContainsKey(Parameters[2]);

        if (productId < 1) return false;

        if (addProduct)
            return _serviceContainer.OrderService.AddProduct(ConsoleUserInterface.AuthenticationData.Token,
                productId, orderId).Result;
        return _serviceContainer.OrderService.DeleteProduct(ConsoleUserInterface.AuthenticationData.Token,
            productId, orderId).Result;
    }

    private bool ArgumentsAreValid(string[] args)
    {
        return TrySetDictionary(args) && (_dict.ContainsKey(Parameters[0]) ||
                                          (_dict.ContainsKey(Parameters[1]) && !_dict.ContainsKey(Parameters[2])));
    }

    private bool TrySetDictionary(string[] args)
    {
        return TryParseArgs(args, out _dict);
    }
}