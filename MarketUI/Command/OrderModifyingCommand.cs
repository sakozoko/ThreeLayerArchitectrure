﻿using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.Objects;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command;

public class OrderModifyingCommand : BaseCommand
{
    private static readonly string[] Parameters = { "-o", "-p",  "-r", "-d", "-s" };
    private readonly IServiceContainer _serviceContainer;
    private Dictionary<string, string> _dict;

    public OrderModifyingCommand(IServiceContainer serviceContainer, IUserInterfaceMapperHandler mapperHandler) :
        base(mapperHandler, Parameters)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        if (!ArgumentsAreValid(args)) return GetHelp();

        _dict.TryGetValue(Parameters[0], out var stringOrderId);

        int.TryParse(stringOrderId, out var orderId);

        var task = _serviceContainer.OrderService.GetById(ConsoleUserInterface.AuthenticationData.Token, orderId);
        var orderModel = Mapper.Map<OrderModel>(task.Result)?? new OrderModel()
        {
            Owner=Mapper.Map<UserModel>(_serviceContainer.UserService.GetByName(ConsoleUserInterface.AuthenticationData.Token, ConsoleUserInterface.AuthenticationData.Name).Result)
        };

        ModifyOrderProducts(orderModel);
        ModifyDesc(orderModel);
        ModifyOrderStatus(orderModel);
        
        return SaveOrder(orderModel) ? $"Order with id {orderId} updated" :
            $"Order with id {orderId} not updated";
    }

    private void ModifyOrderStatus(OrderModel orderModel)
    {
        if (_dict.TryGetValue(Parameters[4], out var stringOrderStatus))
        {
            orderModel.OrderStatus = stringOrderStatus?.Replace(" ","") ?? "New";
        }
    }

    private void ModifyDesc(OrderModel orderModel)
    {
        _dict.TryGetValue(Parameters[3], out var desc);
        if (string.IsNullOrWhiteSpace(desc)) return;
        orderModel.Description = desc;
     }

    private void ModifyOrderProducts(OrderModel orderModel)
    {
        _dict.TryGetValue(Parameters[1], out var stringProductId);

        if (!int.TryParse(stringProductId, out var productId)) return;

        var task =
            _serviceContainer.ProductService.GetById(ConsoleUserInterface.AuthenticationData.Token, productId);
        var addProduct = !_dict.ContainsKey(Parameters[2]);

        var productModel = Mapper.Map<ProductModel>(task.Result);
        
        if (productModel is null) return;

        if (addProduct)
        {
            orderModel.Products.Add(productModel);
        }
        else
        {
            orderModel.Products.Remove(orderModel.Products.FirstOrDefault(x => x.Id == productId));
        }
    }

    private bool SaveOrder(OrderModel order)
    {
        return _serviceContainer.OrderService.
            SaveOrder(ConsoleUserInterface.AuthenticationData.Token, 
                Mapper.Map<Order>(order)).Result;
    }

    private bool ArgumentsAreValid(string[] args)
    {
        return TrySetDictionary(args) && (_dict.ContainsKey(Parameters[0]) || (_dict.ContainsKey(Parameters[1]) && !_dict.ContainsKey(Parameters[2])));
    }

    private bool TrySetDictionary(string[] args)
    {
        return TryParseArgs(args, out _dict);
    }
    public override string GetHelp()
    {
        return "Modyfing order \t mo or modifyorder \t -o, -p,  -r, -d, -s";
    }
}
