using System.Collections.Generic;
using System.Linq;
using BLL;
using ConsoleTable;
using MarketUI.Command.Base;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command.Product;

public class ProductsViewCommand : BaseCommand
{
    private readonly IServiceContainer _serviceContainer;
    private bool _isGroupBy;
    private string _name;

    public ProductsViewCommand(IServiceContainer serviceContainer, IUserInterfaceMapperHandler mapperHandler,
        ICommandsInfoHandler cih) :
        base(mapperHandler, cih)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        TryParseGroupKeyAndName(args);
        var productModels = string.IsNullOrWhiteSpace(_name)
            ? Mapper.Map<IEnumerable<ProductModel>>(_serviceContainer.ProductService
                .GetAll(ConsoleUserInterface.AuthenticationData?.Token).Result)
            : Mapper.Map<IEnumerable<ProductModel>>(_serviceContainer.ProductService.GetByName(_name).Result);
        var consoleTable = new Table().AddColumn("#", "Name", "Product category", "Product cost", "Product description")
            .AddAlignment(Alignment.Center).AddCustomFormat(3, "{0:0.00}");
        if (_isGroupBy)
            ViewProductsGroupedByCategory(productModels, consoleTable);
        else
            ViewProducts(productModels, consoleTable);
        return consoleTable.ToString();
    }

    private static void ViewProducts(IEnumerable<ProductModel> productModels, Table ct)
    {
        foreach (var product in productModels)
            ct.AddRow(product.Id, product.Name, product.Category.Name, product.Cost, product.Description);
    }

    private static void ViewProductsGroupedByCategory(IEnumerable<ProductModel> productModels, Table ct)
    {
        var groups = productModels.GroupBy(x => x.Category);
        foreach (var group in groups)
        {
            var productsOfGroup = group.Select(x => x);
            ViewProducts(productsOfGroup, ct);
        }
    }

    private void TryParseGroupKeyAndName(string[] args)
    {
        _name = string.Empty;
        _isGroupBy = TryParseArgs(args, out var dict) && dict.ContainsKey(Parameters[0]);
        dict?.TryGetValue(Parameters[1], out _name);
    }
}