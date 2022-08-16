using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using BLL.Helpers.Token;
using BLL.Objects;
using BLL.Services.Interfaces;
using BLL.Util.Logger;
using DAL.Repositories;
using Entities;


namespace BLL.Services;

public class UserService : BaseService<UserEntity>, IUserService
{
    public UserService(IRepository<UserEntity> repository, ITokenHandler tokenHandler, ILogger logger, IMapper mapper) :
        base(repository, tokenHandler, logger, mapper)
    {
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest request)
    {
        if (request is null) LogAndThrowAuthenticationException("Request is null");
        var user = Mapper.Map<User>(Repository.GetAll().FirstOrDefault
            (x => x.Name == request.Name && x.Password == request.Password));
        if (user is null) return null;
        Logger.Log($"{user.Name} signed in");
        return CreateAuthenticateResponse(user);
    }

    public AuthenticateResponse Registration(AuthenticateRequest request)
    {
        if (request is null) LogAndThrowAuthenticationException("Request is null");
        if (request.Name.Length < 4 || request.Password.Length < 6)
            return null;
        if (Repository.GetAll().FirstOrDefault(x => x.Name == request.Name) != null)
            LogAndThrowAuthenticationException("Name taken");
        var user = new User { Name = request.Name, Surname = request.Surname, Password = request.Password, Orders = new List<Order>()};
        Repository.Add(Mapper.Map<UserEntity>(user));
        Logger.Log($"{user.Name} registrated.");
        return CreateAuthenticateResponse(user);
    }

    public AuthenticateResponse GetAuthenticateResponse(string token)
    {
        var requestUser = TokenHandler.GetUser(token);
        ThrowAuthenticationExceptionIfUserIsNull(requestUser); 
        return CreateAuthenticateResponse(requestUser);
    }

    private AuthenticateResponse CreateAuthenticateResponse(User user)
    {
        return new AuthenticateResponse(user, TokenHandler.GenerateToken(user));
    }

    public Task<User> GetByName(string token, string name)
    {
        return Task.Factory.StartNew(() =>
        {
            ThrowAuthenticationExceptionIfUserIsNull(TokenHandler.GetUser(token));

            return Mapper.Map<User>(Repository.GetAll().FirstOrDefault(x => x.Name == name));
        });
    }

    public Task<User> GetById(string token, int id)
    {
        return Task.Factory.StartNew(() =>
        {
            ThrowAuthenticationExceptionIfUserIsNull(TokenHandler.GetUser(token));

            return Mapper.Map<User>(Repository.GetById(id));
        });
    }

    public Task<bool> ChangePassword(string token, string value, string oldPsw, User user = null)
    {
        var requestUser= user ?? TokenHandler.GetUser(token);
        return Task<bool>.Factory.StartNew(() => requestUser.Password == oldPsw && ChangeProperty(token, x => x.Password = value, user));
    }

    public Task<bool> ChangeName(string token, string value, User user = null)
    {
        var userWithTheSameName = Mapper.Map<IEnumerable<User>>(Repository.GetAll()?
            .Where(userEntity => userEntity.Name.Equals(value, StringComparison.OrdinalIgnoreCase)));
        return Task<bool>.Factory.StartNew(() => userWithTheSameName?.Count()==0 && ChangeProperty(token, x => x.Name = value, user));
    }

    public Task<bool> ChangeSurname(string token, string value, User user = null)
    {
        return Task<bool>.Factory.StartNew(() => ChangeProperty(token, x => x.Surname = value, user));
    }

    public Task<bool> ChangeIsAdmin(string token, bool value, User user)
    {
        var requestUser = TokenHandler.GetUser(token);
        return Task<bool>.Factory.StartNew(() => user.Id != 1 && requestUser != user && ChangeProperty(token, x => x.IsAdmin = value, user));
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
        Repository.Update(Mapper.Map<UserEntity>(user));
        return true;
    }
    public Task<IEnumerable<User>> GetAll(string token)
    {
        return Task.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);
            Logger.Log($"Admin {requestUser.Name} invoked get all users");
            return Mapper.Map<IEnumerable<User>>(Repository.GetAll());
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


}