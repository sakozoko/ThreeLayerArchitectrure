using BLL.Services.Interfaces;

namespace BLL.Services.Factory;

public interface IServiceContainer
{
    public IProductService ProductService { get; }
    public IUserService UserService { get; }
    public IOrderService OrderService { get; }
    public ICategoryService CategoryService { get; }
}