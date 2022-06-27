using BLL.Util.Helpers.Token;
using BLL.Util.Logger;
using BLL.Util.Services.Interfaces;
using DAL.Util.Repositories;
using Entities;

namespace BLL.Util.Services;

public class UserService : BaseService<User>, IUserService
{
    public UserService(IRepository<User> repository, ITokenHandler tokenHandler, ILogger logger) : base(repository,
        tokenHandler, logger)
    {
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest request)
    {
        if (request is null) LogAndThrowAuthenticationException("Request is null");
        var user = Repository.GetAll().FirstOrDefault
            (x => x.Name == request.Username && x.Password == request.Password);
        if (user is null) return null;
        var token = TokenHandler.GenerateToken(user);
        var response = new AuthenticateResponse(user, token);
        Logger.Log($"{user.Name} signed in");
        return response;
    }

    public AuthenticateResponse Registration(AuthenticateRequest request)
    {
        if (request is null) LogAndThrowAuthenticationException("Request is null");
        if (request.Username.Length < 4 || request.Password.Length < 6)
            return null;
        if (Repository.GetAll().FirstOrDefault(x => x.Name == request.Username) != null)
            LogAndThrowAuthenticationException("Name taken");
        var user = new User { Name = request.Username, Password = request.Password };
        Repository.Add(user);
        var token = TokenHandler.GenerateToken(user);
        var response = new AuthenticateResponse(user, token);
        Logger.Log($"{user.Name} registrated.");
        return response;
    }

    public Task<User> GetByName(string token, string name)
    {
        return Task.Factory.StartNew(() =>
        {
            ThrowAuthenticationExceptionIfUserIsNull(TokenHandler.GetUser(token));

            return Repository.GetAll().ToList().Find(x => x.Name == name);
        });
    }

    public Task<User> GetById(string token, int id)
    {
        return Task.Factory.StartNew(() =>
        {
            ThrowAuthenticationExceptionIfUserIsNull(TokenHandler.GetUser(token));

            return Repository.GetById(id);
        });
    }

    public Task<bool> ChangePassword(string token, string value, User user = null)
    {
        return Task<bool>.Factory.StartNew(() => ChangeProperty(token, x => x.Password = value, user));
    }

    public Task<bool> ChangeName(string token, string value, User user = null)
    {
        return Task<bool>.Factory.StartNew(() => ChangeProperty(token, x => x.Name = value, user));
    }

    public Task<bool> ChangeSurname(string token, string value, User user = null)
    {
        return Task<bool>.Factory.StartNew(() => ChangeProperty(token, x => x.Surname = value, user));
    }

    public Task<bool> ChangeIsAdmin(string token, bool value, User user)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            if (user is null) return false;
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);
            if (requestUser == user) return false;
            if (user.Id == 1) return false;
            user.IsAdmin = value;
            Logger.Log($"Admin {requestUser.Name} changed IsAdmin status for user id {user.Id}");
            return true;
        });
    }

    public Task<IEnumerable<User>> GetAll(string token)
    {
        return Task.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);
            Logger.Log($"Admin {requestUser.Name} invoked get all users");
            return Repository.GetAll();
        });
    }

    public Task<bool> Remove(string token, User entity)
    {
        return Task<bool>.Factory.StartNew(() => entity is not null && Remove(token, entity.Id).Result);
    }

    public Task<bool> Remove(string token, int id)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);
            Logger.Log($"Admin {requestUser.Name} invoked remove user with id {id}");
            return requestUser.Id != id && Repository.Delete(Repository.GetById(id));
        });
    }

    private bool ChangeProperty(string token, Action<User> act, User user = null)
    {
        var requestUser = TokenHandler.GetUser(token);

        ThrowAuthenticationExceptionIfUserIsNull(requestUser);

        if (user is not null && user != requestUser)
        {
            ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
            Logger.Log($"Admin {requestUser.Name} changed property for user id {user.Id}");
        }

        user ??= requestUser;
        act.Invoke(user);
        return true;
    }
}