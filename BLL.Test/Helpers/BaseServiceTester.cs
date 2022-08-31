using AutoMapper;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Objects;
using BLL.Services;
using DAL;
using Entities;

namespace BLL.Test.Helpers;

public class BaseServiceTester : BaseService
{
    public BaseServiceTester(IUnitOfWork unitOfWork, ITokenHandler tokenHandler, ILogger logger, IMapper mapper) : base(
        unitOfWork, tokenHandler, logger, mapper)
    {
    }

    public IUnitOfWork UnitOfWorkTest => UnitOfWork;
    public ITokenHandler TokenHandlerTest => TokenHandler;
    public ILogger LoggerTest => Logger;
    public IMapper MapperTest => Mapper;

    public void LogAndThrowAuthenticationExceptionTest(string msg)
    {
        LogAndThrowAuthenticationException(msg);
    }

    public void ThrowAuthenticationExceptionIfUserIsNullOrNotAdminTest(UserEntity user)
    {
        ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(user);
    }

    public void ThrowAuthenticationExceptionIfUserIsNullTest(UserEntity user)
    {
        ThrowAuthenticationExceptionIfUserIsNull(user);
    }

    public void ThrowAuthenticationExceptionIfUserIsNotAdminTest(UserEntity user)
    {
        ThrowAuthenticationExceptionIfUserIsNotAdmin(user);
    }
}