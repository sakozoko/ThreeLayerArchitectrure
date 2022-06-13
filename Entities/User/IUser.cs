namespace Entities.User;

public interface IUser
{
    public int Id { get; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public bool IsLoggedIn { get; }
    public List<Order> Orders { get; }
    public bool Login(string password);
    public bool ChangePassword(string oldPassword, string newPassword);
    public void CreateNewOrder(string desc);
    public bool Logout();
}