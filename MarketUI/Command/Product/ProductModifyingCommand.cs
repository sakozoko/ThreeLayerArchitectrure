using System.Collections.Generic;
using BLL;
using MarketUI.Command.Base;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command.Product;

public class ProductModifyingCommand : BaseParameterizedCommand
{
    private readonly IServiceManager _serviceManager;
    private Dictionary<string, string> _dict;

    public ProductModifyingCommand(IServiceManager serviceManager, IUserInterfaceMapperHandler mapperHandler,
        ICommandsInfoHandler cih) : base(
        mapperHandler, cih)
    {
        _serviceManager = serviceManager;
    }

    public override string Execute(string[] args)
    {
        if (!TrySetDictionary(args)) GetHelp();
        _dict.TryGetValue(Parameters[0], out var strProductId);
        int.TryParse(strProductId, out var productId);
        return ResultOfTryChange(productId) ? "Product information updated" : "Something is wrong";
    }

    private bool ResultOfTryChange(int productId)
    {
        var firstFlag = TryChangeName(productId);
        var secondFlag = TryChangeDescription(productId);
        var thirdFlag = TryChangeCategory(productId);
        var fourthFlag = TryChangePrice(productId);
        return firstFlag || secondFlag || thirdFlag || fourthFlag;
    }

    private bool TrySetDictionary(string[] args)
    {
        return TryParseArgs(args, out _dict);
    }

    private bool TryChangeName(int productId)
    {
        if (_dict.TryGetValue(Parameters[1], out var newName) && !string.IsNullOrWhiteSpace(newName))
            return _serviceManager.ProductService
                .ChangeName(ConsoleUserInterface.AuthenticationData.Token, newName, productId).Result;
        return false;
    }

    private bool TryChangeDescription(int productId)
    {
        if (_dict.TryGetValue(Parameters[2], out var newDesc) && !string.IsNullOrWhiteSpace(newDesc))
            return _serviceManager.ProductService.ChangeDescription(ConsoleUserInterface.AuthenticationData.Token,
                newDesc, productId).Result;
        return false;
    }

    private bool TryChangeCategory(int productId)
    {
        if (_dict.TryGetValue(Parameters[3], out var newCategory) && !string.IsNullOrWhiteSpace(newCategory))
        {
            var targetCategoryId =
                Mapper.Map<CategoryModel>(_serviceManager.CategoryService
                    .GetByName(ConsoleUserInterface.AuthenticationData.Token, newCategory)
                    .Result)?.Id ?? 0;
            return _serviceManager.ProductService.ChangeCategory(ConsoleUserInterface.AuthenticationData.Token,
                targetCategoryId, productId).Result;
        }

        return false;
    }

    private bool TryChangePrice(int productId)
    {
        if (_dict.TryGetValue(Parameters[4], out var newStrPrice) && decimal.TryParse(newStrPrice, out var newPrice))
            return _serviceManager.ProductService
                .ChangeCost(ConsoleUserInterface.AuthenticationData.Token, newPrice, productId).Result;
        return false;
    }
}