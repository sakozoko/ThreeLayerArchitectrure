
namespace Entities.Users;

public class Administrator : RegisteredUser
{
    public Administrator(int id, string name, string surname, string password) : base(id, name, surname, password)
    {
    }
}