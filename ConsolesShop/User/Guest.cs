namespace ConsolesShop.User;

public class Guest : IUser
{
    public int Id { get; } = -1;
    public string Name { get; set; } = "Guest";
    public string Surname { get; set; }
}