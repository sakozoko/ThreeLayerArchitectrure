using AutoMapper;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Services;
using BLL.Services.Interfaces;
using BLL.Util.Interface;
using DAL;

namespace BLL
{
    public class ServiceManager : IServiceManager
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenHandler;
        private readonly IUnitOfWork _unitOfWork;
        private ICategoryService _categoryService;
        private IOrderService _orderService;
        private IProductService _productService;
        private IUserService _userService;

        public ServiceManager(IUnitOfWork unitOfWork, ILogger logger, ITokenHandler tokenHandler,
            IDomainMapperHandler domainMapperHandler)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _tokenHandler = tokenHandler;
            _mapper = domainMapperHandler.GetMapper();
        }

        public IUserService UserService =>
            _userService ??= new UserService(_unitOfWork, _tokenHandler, _logger, _mapper);

        public IProductService ProductService =>
            _productService ??= new ProductService(_unitOfWork, _tokenHandler, _logger, _mapper);

        public IOrderService OrderService =>
            _orderService ??= new OrderService(_unitOfWork, _tokenHandler, _logger, _mapper);


        public ICategoryService CategoryService =>
            _categoryService ??= new CategoryService(_unitOfWork, _tokenHandler, _logger, _mapper);
    }
}