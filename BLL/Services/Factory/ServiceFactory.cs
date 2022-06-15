using BLL.Helpers;
using BLL.Logger;
using Bll.Services;
using DAL;

namespace BLL.Services.Factory;

public class ServiceFactory : IServiceFactory
{
    private readonly ILogger _logger;
    private readonly CustomTokenHandler _tokenHandler;
    private readonly IUnitOfWork _unitOfWork;
    private IOrderService _orderService;
    private IProductService _productService;
    private IUserService _userService;

    public ServiceFactory(IUnitOfWork unitOfWork, CustomTokenHandler tokenHandler, ILogger logger)
    {
        _tokenHandler = tokenHandler;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public IUserService UserService =>
        _userService ??= new UserService(_unitOfWork.UserRepository, _tokenHandler, _logger);

    public IProductService ProductService =>
        _productService ??= new ProductService(_unitOfWork.ProductRepository, _tokenHandler, _logger);

    public IOrderService OrderService =>
        _orderService ??= new OrderService(_unitOfWork.OrderRepository, _tokenHandler, _logger);
}