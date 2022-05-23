namespace ConsolesShop.User;

public class RegisteredUser : IUser
{
    private string _password;

    public RegisteredUser(string name, string surname, string password)
    {
        Name = name;
        Surname = surname;
        _password = password;
    }

    public bool IsLoggedIn { get; private set; }

    public string Name { get; set; }
    public string Surname { get; set; }

    public bool Login(string password)
    {
        if (password == _password)
            IsLoggedIn = true;
        return IsLoggedIn;
    }

    public bool ChangePassword(string oldPassword, string newPassword)
    {
        if (!IsLoggedIn)
            throw new UserException("Are not logged out");
        if (_password != oldPassword)
            return false;
        _password = newPassword;
        return true;
    }

    public bool Logout()
    {
        if (!IsLoggedIn)
            throw new UserException("Are not logged out");
        IsLoggedIn = false;
        return true;
    }
}