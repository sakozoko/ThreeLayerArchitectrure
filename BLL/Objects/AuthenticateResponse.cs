using Entities;

namespace BLL.Objects;

public class AuthenticateResponse
{
    public AuthenticateResponse(UserEntity user, string token)
    {
        Name = user.Name;
        Surname = user.Surname;
        IsAdmin = user.IsAdmin;
        Token = token;
    }

    public string Name { get; set; }
    public string Surname { get; set; }
    public bool IsAdmin { get; set; }
    public string Token { get; set; }
}