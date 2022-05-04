using System;

namespace ConsolesShop.User;

public class RegisteredUser : IUser
{
    private bool _isLoggedIn;

    private string _password;

    public RegisteredUser(string name, string surname, string password)
    {
        Name = name;
        Surname = surname;
        _password = password;
    }

    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        private set
        {
            _isLoggedIn = value;
            OnChangeIsLoggedIn();
        }
    }

    public string Name { get; set; }
    public string Surname { get; set; }
    public event EventHandler<UserLogInEventArgs> ChangeIsLoggedIn;

    private void OnChangeIsLoggedIn()
    {
        ChangeIsLoggedIn?.Invoke(this, new UserLogInEventArgs
        {
            IsLoggedIn = IsLoggedIn,
            Name = Name,
            Surname = Surname
        });
    }

    public bool Login(string password)
    {
        if (password == _password)
            IsLoggedIn = true;
        return IsLoggedIn;
    }

    public bool ChangePassword(string oldPassword, string newPassword)
    {
        if (!IsLoggedIn)
            throw new UserException("Are not logined");
        if (_password != oldPassword)
            return false;
        _password = newPassword;
        return true;
    }

    public bool Logout()
    {
        if (!IsLoggedIn)
            throw new UserException("Are not logined");
        IsLoggedIn = false;
        return true;
    }
}