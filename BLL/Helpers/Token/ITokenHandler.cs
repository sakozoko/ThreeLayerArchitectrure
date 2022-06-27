using Entities;

namespace BLL.Util.Helpers.Token;

public interface ITokenHandler
{
    public string GenerateToken(User user);
    public bool ValidateToken(string token);
    public User GetUser(string token);
}