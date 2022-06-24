using System.Runtime.CompilerServices;
using BLL.Helpers;
using BLL.Logger;
using BLL.Services.Exception;
using DAL.Repositories;
using Entities;

namespace BLL.Services;

public class BaseService<T> where T : BaseEntity
{
    protected readonly ILogger Logger;
    protected readonly IRepository<T> Repository;
    protected readonly string[] StandardExceptionMessages = { "Token is bad", "Do not have permission" };
    protected readonly CustomTokenHandler TokenHandler;

    protected BaseService(IRepository<T> repository, CustomTokenHandler tokenHandler, ILogger logger)
    {
        Repository = repository;
        TokenHandler = tokenHandler;
        Logger = logger;
    }

    protected void LogAndThrowServiceException(string msg, [CallerMemberName] string callerName = "")
    {
        var ex = new ServiceException(msg, GetType(), callerName);
        Logger.Log(ex);
        throw ex;
    }

    protected void ThrowServiceExceptionIfUserIsNullOrNotAdmin(User requestUser)
    {
        ThrowServiceExceptionIfUserIsNull(requestUser);
        ThrowServiceExceptionIfUserIsNotAdmin(requestUser);
    }

    protected void ThrowServiceExceptionIfUserIsNull(User requestUser)
    {
        if (requestUser is null) LogAndThrowServiceException(StandardExceptionMessages[0]);
    }

    protected void ThrowServiceExceptionIfUserIsNotAdmin(User requestUser)
    {
        if (!requestUser.IsAdmin) LogAndThrowServiceException(StandardExceptionMessages[1]);
    }
}