using System.Collections.Generic;
using BLL;
using MarketUI.Command.Base;
using MarketUI.Util.Interface;

namespace MarketUI.Command.Product;

public class ProductCreatingCommand : BaseCommand
{
    private readonly IServiceManager _serviceManager;
    private Dictionary<string, string> _dict;

    public ProductCreatingCommand(IServiceManager serviceManager, IUserInterfaceMapperHandler mapperHandler,
        ICommandsInfoHandler commandsInfo) : base(mapperHandler, commandsInfo)
    {
        _serviceManager = serviceManager;
    }

    public override string Execute(string[] args)
    {
        if (!TrySetDictionary(args)) GetHelp();
        if (TryParseName() && TryParseDesc() &&
            ContainsCategoryId() && int.TryParse(_dict[Parameters[2]], out var categoryId) &&
            ContainsCost() && decimal.TryParse(_dict[Parameters[3]], out var cost))
        {
            var newProductId = _serviceManager.ProductService.Create(ConsoleUserInterface.AuthenticationData.Token,
                _dict[Parameters[0]], _dict[Parameters[1]], cost, categoryId).Result;
            if (newProductId != -1)
                return $"Successful! New product id is {newProductId}";
        }

        return "Something wrong";
    }

    private bool TrySetDictionary(string[] args)
    {
        return TryParseArgs(args, out _dict);
    }

    private bool TryParseName()
    {
        return _dict.TryGetValue(Parameters[0], out var name) && !string.IsNullOrWhiteSpace(name);
    }

    private bool TryParseDesc()
    {
        return _dict.TryGetValue(Parameters[1], out var desc) && !string.IsNullOrWhiteSpace(desc);
    }

    private bool ContainsCategoryId()
    {
        return _dict.ContainsKey(Parameters[2]);
    }

    private bool ContainsCost()
    {
        return _dict.ContainsKey(Parameters[3]);
    }
}