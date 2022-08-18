using System;
using System.Collections.Generic;
using BLL;
using BLL.Objects;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command;

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

        var task = _serviceContainer.OrderService.GetById(ConsoleUserInterface.AuthenticationData.Token, orderId);
        var orderModel = Mapper.Map<OrderModel>(task.Result) ?? new OrderModel
        {
            Owner = Mapper.Map<UserModel>(_serviceContainer.UserService
                .GetByName(ConsoleUserInterface.AuthenticationData.Token, ConsoleUserInterface.AuthenticationData.Name)
                .Result)
        };

        if (TryChangeOrderProducts(orderModel) | TryChangeDescription(orderModel) | TryChangeOrderStatus(orderModel))
            return $"Order with id {orderId} updated";

        return "Something wrong";
    }

    private bool TryChangeOrderStatus(OrderModel orderModel)
    {
        return _dict.TryGetValue(Parameters[4], out var stringOrderStatus) &&
               _serviceContainer.OrderService.ChangeOrderStatus(ConsoleUserInterface.AuthenticationData.Token,
                       Enum.Parse<OrderStatus>(stringOrderStatus?.Replace(" ", "") ?? "New"),
                       Mapper.Map<Order>(orderModel))
                   .Result;
    }

    private bool TryChangeDescription(OrderModel orderModel)
    {
        return _dict.TryGetValue(Parameters[3], out var desc) && !string.IsNullOrWhiteSpace(desc) &&
               _serviceContainer.OrderService.ChangeDescription(ConsoleUserInterface.AuthenticationData.Token, desc,
                   Mapper.Map<Order>(orderModel)).Result;
    }

    private bool TryChangeOrderProducts(OrderModel orderModel)
    {
        _dict.TryGetValue(Parameters[1], out var stringProductId);

        if (!int.TryParse(stringProductId, out var productId)) return false;

        var task =
            _serviceContainer.ProductService.GetById(ConsoleUserInterface.AuthenticationData.Token, productId);
        var addProduct = !_dict.ContainsKey(Parameters[2]);

        var productModel = Mapper.Map<ProductModel>(task.Result);

        if (productModel is null) return false;

        if (addProduct)
            return _serviceContainer.OrderService.AddProduct(ConsoleUserInterface.AuthenticationData.Token,
                Mapper.Map<Product>(productModel), Mapper.Map<Order>(orderModel)).Result;
        return _serviceContainer.OrderService.DeleteProduct(ConsoleUserInterface.AuthenticationData.Token,
            Mapper.Map<Product>(productModel), Mapper.Map<Order>(orderModel)).Result;
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