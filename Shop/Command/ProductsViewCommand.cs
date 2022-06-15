using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Entities.Goods;

namespace Shop.Command;

public class ProductsViewCommand : BasicCommand
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

    public override Task<string> Execute(string[] args)
    {
        return Task<string>.Factory.StartNew(() =>
        {
            var stringBuilder = new StringBuilder();
            TryParseGroupKeyAndName(args);
            var task = string.IsNullOrEmpty(_name) ? 
                _service.Factory.ProductService.GetAllProducts(Shop.AuthenticationData?.Token) : 
                _service.Factory.ProductService.GetProductsByName(_name);
                stringBuilder.Append("# Name \t Product category \t Product cost \t Product description");
            stringBuilder.Append(
                _isGroupBy ? ViewProductsGroupedByCategory(task).Result : ViewProducts(task).Result);
            return stringBuilder.ToString();
        });
    }

    private Task<string> ViewProducts(Task<IEnumerable<Product>> task)
    {
        return Task<string>.Factory.StartNew(() =>
        {
            var stringBuilder = new StringBuilder();
            foreach (var product in task.Result) stringBuilder.Append("\n" + product);

            return stringBuilder.ToString();
        });
    }

    private Task<string> ViewProductsGroupedByCategory(Task<IEnumerable<Product>> task)
    {
        return Task<string>.Factory.StartNew(() =>
        {
            var stringBuilder = new StringBuilder();
            var groups =
                task.Result.GroupBy(x => x.Category);
            foreach (var group in groups)
            {
                var productsOfGroup = group.Select(x => x);
                foreach (var product in productsOfGroup) stringBuilder.Append("\n" + product);
            }

            return stringBuilder.ToString();
        });
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