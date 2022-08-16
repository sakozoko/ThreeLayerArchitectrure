using BLL.Objects;

namespace BLL.Services.Interfaces;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest request);
    AuthenticateResponse Registration(AuthenticateRequest request);
    AuthenticateResponse GetAuthenticateResponse(string token);
    Task<User> GetByName(string token, string name);
    Task<User> GetById(string token, int id);
    Task<bool> ChangePassword(string token, string value, string oldPsw, User user = null);
    Task<bool> ChangeName(string token, string value, User user = null);
    Task<bool> ChangeSurname(string token, string value, User user = null);
    Task<bool> ChangeIsAdmin(string token, bool value, User user);
    Task<IEnumerable<User>> GetAll(string token);
    Task<bool> Remove(string token, int id);
    Task<bool> Remove(string token, User entity);
}