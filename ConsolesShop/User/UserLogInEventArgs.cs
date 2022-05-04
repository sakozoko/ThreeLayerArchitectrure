using System;

namespace ConsolesShop.User;

public class UserLogInEventArgs : EventArgs
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserType { get; set; }
    public bool IsLoggedIn { get; set; }
}