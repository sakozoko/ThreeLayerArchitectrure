using BLL.Services.Interfaces;

namespace BLL
{
    public interface IServiceManager
    {
        public IProductService ProductService { get; }
        public IUserService UserService { get; }
        public IOrderService OrderService { get; }
        public ICategoryService CategoryService { get; }
    }
}