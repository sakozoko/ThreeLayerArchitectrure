namespace BLL.Services.Factory;

public interface IServiceFactory
{
    public IProductService ProductService { get; }
    public IUserService UserService { get; }
    public IOrderService OrderService { get; }
}