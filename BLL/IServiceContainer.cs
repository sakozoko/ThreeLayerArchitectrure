using BLL.Util.Services.Interfaces;

namespace BLL.Util;

public interface IServiceContainer
{
    public IProductService ProductService { get; }
    public IUserService UserService { get; }
    public IOrderService OrderService { get; }
    public ICategoryService CategoryService { get; }
}