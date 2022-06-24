using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Entities.Goods;

namespace Shop.Command;

public class ProductsViewCommand : BaseCommand
{
    private static readonly string[] Names = { "view product", "vp" };
    private static readonly string[] Parameters = { "-g", "-n" };
    private readonly Service _service;
    private bool _isGroupBy;
    private string _name;

    public ProductsViewCommand(Service service) : base(Names, Parameters)
    {
        _service = service;
    }

    public override string Execute(string[] args)
    {
        var stringBuilder = new StringBuilder();
        TryParseGroupKeyAndName(args);
        var task = string.IsNullOrEmpty(_name)
            ? _service.Factory.ProductService.GetAll(Shop.AuthenticationData?.Token)
            : _service.Factory.ProductService.GetByName(_name);
        stringBuilder.Append("# Name \t \t Product category \t Product cost \t Product description");
        stringBuilder.Append(_isGroupBy ? ViewProductsGroupedByCategory(task) : ViewProducts(task));
        return stringBuilder.ToString();
    }

    private static string ViewProducts(Task<IEnumerable<Product>> task)
    {
        var stringBuilder = new StringBuilder();
        foreach (var product in task.Result)
            stringBuilder.Append(
                $"\n {product.Id} \t {product.Name} \t {product.Category.Name} \t {product.Cost} \t {product.Description}");
        return stringBuilder.ToString();
    }

    private static string ViewProductsGroupedByCategory(Task<IEnumerable<Product>> task)
    {
        var stringBuilder = new StringBuilder();
        var groups = task.Result.GroupBy(x => x.Category);
        foreach (var group in groups)
        {
            var productsOfGroup = group.Select(x => x);
            foreach (var product in productsOfGroup)
                stringBuilder.Append(
                    $"\n {product.Id} \t {product.Name} \t {product.Category.Name} \t {product.Cost} \t {product.Description}");
        }

        return stringBuilder.ToString();
    }

    private void TryParseGroupKeyAndName(string[] args)
    {
        _name = string.Empty;
        _isGroupBy = TryParseArgs(args, out var dict) && dict.ContainsKey(Parameters[0]);
        dict?.TryGetValue(Parameters[1], out _name);
    }


    public override string GetHelp()
    {
        return "View products \t view products or vp \t none";
    }
}