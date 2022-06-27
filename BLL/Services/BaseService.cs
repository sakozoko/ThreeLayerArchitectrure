using System.Runtime.CompilerServices;
using BLL.Util.Helpers.Token;
using BLL.Util.Logger;
using BLL.Util.Services.Exception;
using DAL.Util.Repositories;
using Entities;

namespace BLL.Util.Services;

public class BaseService<T> where T : BaseEntity
{
    protected readonly ILogger Logger;
    protected readonly IRepository<T> Repository;
    protected readonly ITokenHandler TokenHandler;

    protected BaseService(IRepository<T> repository, ITokenHandler tokenHandler, ILogger logger)
    {
        Repository = repository;
        TokenHandler = tokenHandler;
        Logger = logger;
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

    protected void ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(User requestUser,
        [CallerMemberName] string callerName = "")
    {
        ThrowAuthenticationExceptionIfUserIsNull(requestUser, callerName);
        ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser, callerName);
    }

    protected void ThrowAuthenticationExceptionIfUserIsNull(User requestUser, [CallerMemberName] string callerName = "")
    {
        if (requestUser is null) LogAndThrowAuthenticationException("Token is bad", callerName);
    }

    protected void ThrowAuthenticationExceptionIfUserIsNotAdmin(User requestUser,
        [CallerMemberName] string callerName = "")
    {
        if (!requestUser.IsAdmin) LogAndThrowAuthenticationException("Do not have permission", callerName);
    }
}