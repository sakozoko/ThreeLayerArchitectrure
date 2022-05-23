namespace ConsolesShop.User;

public interface IUser
{
    public int Id { get; }
    public string Name { get; set; }
    public string Surname { get; set; }
    
}