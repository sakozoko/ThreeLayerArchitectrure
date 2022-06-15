namespace Entities.Users;

public interface User
{
    
    public bool Login(string password);
    public bool ChangePassword(string oldPassword, string newPassword);
    public void CreateNewOrder(string desc);
    public bool Logout();
}