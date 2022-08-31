using System.Runtime.CompilerServices;
using AutoMapper;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Services.Exception;
using DAL;
using Entities;

namespace BLL.Services;

public class BaseService
{
    protected readonly ILogger Logger;
    protected readonly IMapper Mapper;
    protected readonly ITokenHandler TokenHandler;
    protected readonly IUnitOfWork UnitOfWork;

    protected BaseService(IUnitOfWork unitOfWork, ITokenHandler tokenHandler, ILogger logger, IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        TokenHandler = tokenHandler;
        Logger = logger;
        Mapper = mapper;
    }

    protected void LogAndThrowAuthenticationException(string msg, [CallerMemberName] string callerName = "")
    {
        var ex = new AuthenticationException(msg)
        {
            Data =
            {
                ["type"] = GetType(),
                ["callerName"] = callerName
            }
        };
        Logger.Log(ex);
        throw ex;
    }

    protected void ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(UserEntity requestUserEntity,
        [CallerMemberName] string callerName = "")
    {
        ThrowAuthenticationExceptionIfUserIsNull(requestUserEntity, callerName);
        ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUserEntity, callerName);
    }

    protected void ThrowAuthenticationExceptionIfUserIsNull(UserEntity requestUserEntity,
        [CallerMemberName] string callerName = "")
    {
        if (requestUserEntity is null) LogAndThrowAuthenticationException("Token is bad", callerName);
    }

    
    protected void ThrowAuthenticationExceptionIfUserIsNotAdmin(UserEntity requestUserEntity,
        [CallerMemberName] string callerName = "")
    {
        if (!requestUserEntity.IsAdmin) LogAndThrowAuthenticationException("Do not have permission", callerName);
    }
}