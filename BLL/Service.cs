using BLL.Helpers;
using BLL.Logger;
using BLL.Services.Factory;
using DAL;

namespace BLL;

public class Service
{
    private readonly ILogger _logger;
    private readonly CustomTokenHandler _tokenHandler;
    private readonly IUnitOfWork _unitOfWork = new UnitOfWork();
    private IServiceFactory _serviceFactory;

    public Service() : this(new DebugLogger())
    {
    }

    public Service(ILogger logger)
    {
        _logger = logger;
        _tokenHandler = new CustomTokenHandler(_unitOfWork.UserRepository, _logger);
    }

    public IServiceFactory Factory =>
        _serviceFactory ??= new ServiceFactory(_unitOfWork, _tokenHandler, _logger);
}