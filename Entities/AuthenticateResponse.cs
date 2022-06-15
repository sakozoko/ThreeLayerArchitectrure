namespace Entities;

public class AuthenticateResponse
{
    public AuthenticateResponse(User user, string token)
    {
        Id = user.Id;
        Name = user.Name;
        Surname = user.Surname;
        IsAdmin = user.IsAdmin;
        Token = token;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public bool IsAdmin { get; set; }
    public string Token { get; set; }
}