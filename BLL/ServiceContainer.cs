using BLL.Helpers.Token;
using BLL.Services;
using BLL.Services.Interfaces;
using BLL.Util.Logger;
using DAL;

namespace BLL;

public class ServiceContainer : IServiceContainer
{
    private readonly ILogger _logger;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUnitOfWork _unitOfWork;
    private ICategoryService _categoryService;
    private IOrderService _orderService;
    private IProductService _productService;
    private IUserService _userService;

    public ServiceContainer(IUnitOfWork unitOfWork, ILogger logger, ITokenHandler tokenHandler)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _tokenHandler = tokenHandler;
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