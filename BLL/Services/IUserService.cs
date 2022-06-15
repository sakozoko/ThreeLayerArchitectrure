using Entities;

namespace BLL.Services;

public interface IUserService
{
    public AuthenticateResponse Authenticate(AuthenticateRequest request);
    public AuthenticateResponse Registration(AuthenticateRequest request);
    public Task<User> GetByName(string token, string name);
    public Task<User> GetById(string token,int id);
    public Task<bool> ChangePassword(string token,  string newPwd,User user=null);
    public Task<bool> ChangeName(string token, string newName, User user = null);
    public Task<bool> ChangeSurname(string token, string newSurname, User user = null);
    public Task<bool> ChangeIsAdmin(string token, bool value,User user);
    public Task<IEnumerable<User>> GetAllUsers(string token);
}