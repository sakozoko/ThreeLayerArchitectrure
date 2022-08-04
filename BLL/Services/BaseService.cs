using System.Runtime.CompilerServices;
using AutoMapper;
using BLL.Helpers.Token;
using BLL.Objects;
using BLL.Services.Exception;
using BLL.Util.Interface;
using BLL.Util.Logger;
using DAL.Repositories;
using Entities;

namespace BLL.Services;

public class BaseService<T> where T : BaseEntity
{
    protected readonly ILogger Logger;
    protected readonly IRepository<T> Repository;
    protected readonly ITokenHandler TokenHandler;
    protected readonly IMapper Mapper;
    protected BaseService(IRepository<T> repository, ITokenHandler tokenHandler, ILogger logger, IMapper mapper)
    {
        Repository = repository;
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

    protected void ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(User requestUserEntity,
        [CallerMemberName] string callerName = "")
    {
        ThrowAuthenticationExceptionIfUserIsNull(requestUserEntity, callerName);
        ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUserEntity, callerName);
    }

    protected void ThrowAuthenticationExceptionIfUserIsNull(User requestUserEntity,
        [CallerMemberName] string callerName = "")
    {
        if (requestUserEntity is null) LogAndThrowAuthenticationException("Token is bad", callerName);
    }

    protected void ThrowAuthenticationExceptionIfUserIsNotAdmin(User requestUserEntity,
        [CallerMemberName] string callerName = "")
    {
        if (!requestUserEntity.IsAdmin) LogAndThrowAuthenticationException("Do not have permission", callerName);
    }
}