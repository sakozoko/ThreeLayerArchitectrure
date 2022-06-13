using DAL.DataContext;
using Entities.User;

namespace DAL.Repositories;

public class UserRepository : IRepository<IUser>
{
    private readonly DbContext _dbContext;
    private readonly List<IUser> _users;
    
    public UserRepository(DbContext dbcontext)
    {
        _dbContext = dbcontext;
        _users = dbcontext.Users;
    }

    public void Add(IUser entity)
    {
        _users.Add(entity);
    }

    public IEnumerable<IUser> GetAll()
    {
        return _users;
    }

    public IUser GetById(int id)
    {
        return _users.Find(x => x.Id == id);
    }
    
    public void Delete(IUser entity)
    {
        _users.Remove(entity);
    }
}