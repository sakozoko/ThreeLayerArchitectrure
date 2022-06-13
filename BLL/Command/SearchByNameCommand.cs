using DAL.Repositories;
using Entities.Goods;
using Entities.User;

namespace BLL.Command;

public class SearchByNameCommand : BasicCommand
{
    private static readonly string[] Names = { "findby", "f", "s" };
    private readonly IRepository<Product> _productRepository;

    public SearchByNameCommand(IRepository<Product> productRepository) : base(Names)
    {
        _productRepository = productRepository;
    }

    public override bool CanExecute(IUser user, string[] args = null)
    {
        return true;
    }

    public override Task<string> Execute(string[] args)
    {
        return Task<string>.Factory.StartNew(() =>"SearchByName is fine");
    }

    public override Task<string> GetHelp()
    {
        return Task<string>.Factory.StartNew(() =>"Search by name command \t findby, f, s \t none");
    }
}