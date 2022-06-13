namespace Entities.User;

public class RegisteredUser : IUser
{
    private string _password;

    public RegisteredUser(int id, string name, string surname, string password)
    {
        Id = id;
        Name = name;
        Surname = surname;
        _password = password;
        Orders = new List<Order>();
    }

    public List<Order> Orders { get; }

    public bool IsLoggedIn { get; private set; }

    public int Id { get; }
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

    public void CreateNewOrder(string desc)
    {
        Orders.Add(new Order(desc, this));
    }

    public bool Logout()
    {
        if (!IsLoggedIn)
            throw new UserException("Are not logged out");
        IsLoggedIn = false;
        return true;
    }

    public override bool Equals(object obj)
    {
        if (obj is null or not IUser) return false;
        return Id == ((IUser)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id;
    }
}