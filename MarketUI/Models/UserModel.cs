namespace MarketUI.Models;

public class UserModel : BaseModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public bool IsAdmin { get; set; }
}