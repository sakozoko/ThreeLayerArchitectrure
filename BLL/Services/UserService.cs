using BLL.Helpers;
using BLL.Logger;
using Bll.Services;
using DAL.Repositories;
using Entities;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly ILogger _logger;
    private readonly CustomTokenHandler _tokenHandler;
    private readonly IRepository<User> _userRepository;
    private const string Msg= "Do not have permission to it";
    public UserService(IRepository<User> userRepository, CustomTokenHandler tokenHandler, ILogger logger)
    {
        _tokenHandler = tokenHandler;
        _userRepository = userRepository;
        _logger = logger;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest request)
    {
        if (request is null)
        {
            const string msg = "Request is null";
            _logger.LogException($"{nameof(UserService)}.{nameof(Authenticate)} throw exception. " + msg);
            throw new ServiceException(nameof(UserService), msg);
        }
        var user = _userRepository.GetAll().FirstOrDefault
            (x => x.Name == request.Username && x.Password == request.Password);
        if (user is null) return null;
        var token = _tokenHandler.GenerateToken(user);
        var response = new AuthenticateResponse(user, token);
        _logger.Log($"{user.Name} signed in");
        return response;
    }

    public AuthenticateResponse Registration(AuthenticateRequest request)
    {
        if (request is null)
        {
            const string msg = "Request is null";
            _logger.LogException($"{nameof(UserService)}.{nameof(Authenticate)} throw exception. " + msg);
            throw new ServiceException(nameof(UserService), msg);
        }
        if (request.Username.Length < 4 || request.Password.Length < 6)
            return null;
        if (_userRepository.GetAll().FirstOrDefault(x => x.Name == request.Username) != null)
        {
            const string msg = "Name taken";
            _logger.LogException($"{nameof(UserService)}.{nameof(Authenticate)} throw exception. " + msg);
            throw new ServiceException(nameof(UserService), msg);
        }
        var user = new User{ Name = request.Username, Password = request.Password};
        _userRepository.Add(user);
        var token = _tokenHandler.GenerateToken(user);
        var response = new AuthenticateResponse(user, token);
        _logger.Log($"{user.Name} registrated.");
        return response;
    }
    public Task<User> GetByName(string token,string name)
    {
        return Task.Factory.StartNew(() =>
        {
            if(_tokenHandler.ValidateToken(token))
                return _userRepository.GetAll().ToList().Find(x => x.Name == name);
            var msg = $"Token is bad \n{token}";
            _logger.LogException($"{nameof(OrderService)}.{nameof(GetByName)} throw exception. " + msg);
            throw new ServiceException(nameof(UserService), msg);
        });
    }

    public Task<User> GetById(string token, int id)
    {
        if(_tokenHandler.ValidateToken(token))
            return Task.Factory.StartNew(() => _userRepository.GetById(id));
        const string msg = $"Token is bad ";
        _logger.LogException($"{nameof(OrderService)}.{nameof(GetByName)} throw exception. " + msg);
        throw new ServiceException(nameof(UserService), msg);
    }

    private bool ChangeProperty(string token,  Action<User> act, User user = null)
    {
        var requestUser = _tokenHandler.GetUser(token);
        if (requestUser is not null)
        {
            if (user is null)
            {
                act.Invoke(requestUser);
                return true;
            }

            if (user == requestUser)
            {
                act.Invoke(user);
                return true;
            }

            if (requestUser.IsAdmin)
            {
                act.Invoke(user);
                _logger.Log($"Admin {requestUser.Name} changed property for user id {user.Id}");
                return true;
            }
            _logger.LogException($"{nameof(UserService)}.{nameof(ChangeProperty)} throw exception. " + Msg);
            throw new ServiceException(nameof(UserService), Msg);
        }
        _logger.LogException($"{nameof(UserService)}.{nameof(ChangeProperty)} throw exception. Token is bad");
        throw new ServiceException(nameof(UserService), "Token is bad"); 
    }
    public Task<bool> ChangePassword(string token, string value,User user=null) => 
        Task<bool>.Factory.StartNew(() => ChangeProperty(token, x => x.Password = value, user));

    public Task<bool> ChangeName(string token, string value, User user=null) =>
        Task<bool>.Factory.StartNew(() => ChangeProperty(token, x => x.Name = value, user));

    public Task<bool> ChangeSurname(string token, string value, User user=null) =>
        Task<bool>.Factory.StartNew(() => ChangeProperty(token, x => x.Surname = value, user));

    public Task<bool> ChangeIsAdmin(string token, bool value,User user)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            var requestUser = _tokenHandler.GetUser(token);
            if (requestUser is {IsAdmin:true})
            {
                if (user is null) return false;
                user.IsAdmin = value;
                _logger.Log($"Admin {requestUser.Name} changed IsAdmin status for user id {user.Id}");
                return false;
            }

            if (requestUser is not null)
            {
                _logger.LogException($"{nameof(UserService)}.{nameof(ChangeIsAdmin)} throw exception. " + Msg);
                throw new ServiceException(nameof(UserService), Msg);
            }
            _logger.LogException($"{nameof(UserService)}.{nameof(ChangeIsAdmin)} throw exception. Token is bad");
            throw new ServiceException(nameof(UserService), "Token is bad");
        });
    }
    


    public Task<IEnumerable<User>> GetAllUsers(string token)
    {
        return Task.Factory.StartNew(() =>
        {
            var user = _tokenHandler.GetUser(token);
            if (user is { IsAdmin: true })
            {
                _logger.Log($"Admin {user.Name} invoked to get all users");
                return _userRepository.GetAll();
            }
            _logger.LogException($"{nameof(UserService)}.{nameof(GetAllUsers)} throw exception. " + Msg);
            throw new ServiceException(nameof(UserService), Msg);
        });
    }

    public Task<bool> Delete(string token,User user=null)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            if (user is null)
                return false;
            _userRepository.Delete(user);
            return true;
        });
    }
}