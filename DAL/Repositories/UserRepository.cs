using DAL.DataContext;
using Entities;

namespace DAL.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly DbContext _dbContext;
    private readonly List<User> _users;
    private int _lastId;

    public UserRepository(DbContext dbcontext)
    {
        _dbContext = dbcontext;
        _users = dbcontext.Users;
        _lastId = _users.Count;
    }

    public int Add(User entity)
    {
        entity.Id = ++_lastId;
        _users.Add(entity);
        return _lastId;
    }

    public IEnumerable<User> GetAll()
    {
        return _users;
    }

    public User GetById(int id)
    {
        return _users.Find(x => x.Id == id);
    }

    public void Delete(User entity)
    {
        _users.Remove(entity);
    }
}