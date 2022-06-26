using Entities;

namespace BLL.Helpers.Token;

public interface ITokenHandler
{
    public string GenerateToken(User user);
    public bool ValidateToken(string token);
    public User GetUser(string token);
}