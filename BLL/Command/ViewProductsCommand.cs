using System.Text;
using DAL.Repositories;
using Entities.Goods;
using Entities.User;

namespace BLL.Command;

public class ViewProductsCommand : BasicCommand
{
    private static readonly string[] Names = { "view product", "vp" };
    private static readonly string[] Param = { "-g" };
    private readonly IRepository<Product> _productRepository;
    private bool _groupKey;

    public ViewProductsCommand(IRepository<Product> productRepository) : base(Names, Param)
    {
        _productRepository = productRepository;
    }

    public override bool CanExecute(IUser user, string[] args)
    {
        TryParseGroupKey(args);
        return user is not null;
    }

    public override Task<string> Execute(string[] args=null)
    {
        return Task<string>.Factory.StartNew(() =>
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("\n# Name \t Product category \t Product cost \t Product description");
            stringBuilder.Append(!_groupKey ? ViewProducts().Result : ViewProductsGroupedByCategory().Result);
            return stringBuilder.ToString();
        });
    }

    private Task<string> ViewProducts()
    {
        return Task<string>.Factory.StartNew(() =>
        {
            var stringBuilder = new StringBuilder();
            foreach (var product in _productRepository.GetAll())
            {
                stringBuilder.Append("\n"+product);
            }

            return stringBuilder.ToString();
        });
    }

    private Task<string> ViewProductsGroupedByCategory()
    {
        return Task<string>.Factory.StartNew(() =>
        {
            var stringBuilder = new StringBuilder();
            var groups = _productRepository.GetAll().GroupBy(x => x.Category);
            foreach (var group in groups)
            {
                var productsOfGroup = group.Select(x => x);
                foreach (var product in productsOfGroup)
                {
                    stringBuilder.Append("\n" + product);
                }
            }

            return stringBuilder.ToString();
        });

    }

    private bool TryParseGroupKey(string[] args)
    {
        if (TryParseArgs(args, out var dict) && dict.ContainsKey(Param[0]))
            _groupKey = true;
        else
            _groupKey = false;

        return _groupKey;
    }

    public override Task<string> GetHelp()
    {
        return Task<string>.Factory.StartNew(() =>"View products \t view products or vp \t none");
    }
}