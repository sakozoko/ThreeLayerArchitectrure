using BLL.Objects;

namespace BLL.Helpers.Token;

public interface ITokenHandler
{
    public string GenerateToken(User userEntity);
    public bool ValidateToken(string token);
    public User GetUser(string token);
}