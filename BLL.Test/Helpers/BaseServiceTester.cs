using AutoMapper;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Objects;
using BLL.Services;
using DAL;

namespace BLL.Test.Helpers;

public class BaseServiceTester : BaseService
{
    public IUnitOfWork UnitOfWorkTest => UnitOfWork;
    public ITokenHandler TokenHandlerTest => TokenHandler;
    public ILogger LoggerTest => Logger;
    public IMapper MapperTest => Mapper;
    
    public BaseServiceTester(IUnitOfWork unitOfWork, ITokenHandler tokenHandler, ILogger logger, IMapper mapper) : base(unitOfWork, tokenHandler, logger, mapper)
    {
    }

    public void LogAndThrowAuthenticationExceptionTest(string msg)
    {
        LogAndThrowAuthenticationException(msg);
    }

    public void ThrowAuthenticationExceptionIfUserIsNullOrNotAdminTest(User user)
    {
        ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(user);
    }

    public void ThrowAuthenticationExceptionIfUserIsNullTest(User user)
    {
        ThrowAuthenticationExceptionIfUserIsNull(user);
    }

    public void ThrowAuthenticationExceptionIfUserIsNotAdminTest(User user)
    {
        ThrowAuthenticationExceptionIfUserIsNotAdmin(user);
    }
    
}