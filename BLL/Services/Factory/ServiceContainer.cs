using BLL.Helpers;
using BLL.Logger;
using BLL.Services.Interfaces;
using DAL;

namespace BLL.Services.Factory;

public class ServiceContainer : IServiceContainer
{
    private readonly ILogger _logger;
    private readonly CustomTokenHandler _tokenHandler;
    private readonly IUnitOfWork _unitOfWork = new UnitOfWork();
    private ICategoryService _categoryService;
    private IOrderService _orderService;
    private IProductService _productService;
    private IUserService _userService;

    public ServiceContainer() : this(new DebugLogger())
    {
    }

    public ServiceContainer(ILogger logger)
    {
        _logger = logger;
        _tokenHandler = new CustomTokenHandler(_unitOfWork.UserRepository, _logger);
    }

    public IUserService UserService =>
        _userService ??= new UserService(_unitOfWork.UserRepository, _tokenHandler, _logger);

    public IProductService ProductService =>
        _productService ??= new ProductService(_unitOfWork.ProductRepository, _tokenHandler, _logger);

    public IOrderService OrderService =>
        _orderService ??= new OrderService(_unitOfWork.OrderRepository, _tokenHandler, _logger);


    public ICategoryService CategoryService =>
        _categoryService ??= new CategoryService(_unitOfWork.CategoryRepository, _tokenHandler, _logger);
}