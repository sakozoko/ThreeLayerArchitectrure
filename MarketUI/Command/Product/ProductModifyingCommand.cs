using System.Collections.Generic;
using BLL;
using MarketUI.Command.Base;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command.Product;

public class ProductModifyingCommand : BaseCommand
{
    private readonly IServiceContainer _serviceContainer;
    private Dictionary<string, string> _dict;

    public ProductModifyingCommand(IServiceContainer serviceContainer, IUserInterfaceMapperHandler mapperHandler,
        ICommandsInfoHandler cih) : base(
        mapperHandler, cih)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        if (!TrySetDictionary(args)) GetHelp();
        _dict.TryGetValue(Parameters[0], out var strProductId);
        int.TryParse(strProductId, out var productId);
        var targetProductModel = Mapper.Map<ProductModel>(
            _serviceContainer.ProductService.GetById(ConsoleUserInterface.AuthenticationData.Token, productId).Result);
        if (targetProductModel is null)
            return "Target product not found";
        if (TryChangeName(targetProductModel) | TryChangeDescription(targetProductModel) |
            TryChangeCategory(targetProductModel) | TryChangePrice(targetProductModel))
            return "Product information updated";
        return "Something is wrong";
    }

    private bool TrySetDictionary(string[] args)
    {
        return TryParseArgs(args, out _dict);
    }

    private bool TryChangeName(ProductModel productModel)
    {
        if (_dict.TryGetValue(Parameters[1], out var newName) && !string.IsNullOrWhiteSpace(newName))
            return _serviceContainer.ProductService.ChangeName(ConsoleUserInterface.AuthenticationData.Token, newName,
                Mapper.Map<BLL.Objects.Product>(productModel)).Result;
        return false;
    }

    private bool TryChangeDescription(ProductModel productModel)
    {
        if (_dict.TryGetValue(Parameters[2], out var newDesc) && !string.IsNullOrWhiteSpace(newDesc))
            return _serviceContainer.ProductService.ChangeDescription(ConsoleUserInterface.AuthenticationData.Token,
                newDesc,
                Mapper.Map<BLL.Objects.Product>(productModel)).Result;
        return false;
    }

    private bool TryChangeCategory(ProductModel productModel)
    {
        if (_dict.TryGetValue(Parameters[3], out var newCategory) && !string.IsNullOrWhiteSpace(newCategory))
        {
            var targetCategory =
                _serviceContainer.CategoryService.GetByName(ConsoleUserInterface.AuthenticationData.Token, newCategory)
                    .Result;
            return _serviceContainer.ProductService.ChangeCategory(ConsoleUserInterface.AuthenticationData.Token,
                targetCategory,
                Mapper.Map<BLL.Objects.Product>(productModel)).Result;
        }

        return false;
    }

    private bool TryChangePrice(ProductModel productModel)
    {
        if (_dict.TryGetValue(Parameters[4], out var newStrPrice) && decimal.TryParse(newStrPrice, out var newPrice))
            return _serviceContainer.ProductService.ChangeCost(ConsoleUserInterface.AuthenticationData.Token, newPrice,
                Mapper.Map<BLL.Objects.Product>(productModel)).Result;
        return false;
    }
}