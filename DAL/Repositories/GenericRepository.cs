using DAL.DataContext;
using Entities;

namespace DAL.Repositories;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DbContext _dbContext;
    private readonly IList<T> _entities;
    private int _lastId;

    public GenericRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _entities = dbContext.Set<T>();
        _lastId = _entities.Count;
    }

    public int Add(T entity)
    {
        entity.Id = ++_lastId;
        _entities.Add(entity);
        return _lastId;
    }

    public IEnumerable<T> GetAll()
    {
        return _entities;
    }

    public T GetById(int id)
    {
        return _entities.FirstOrDefault(x => x.Id == id);
    }

    public bool Delete(T entity)
    {
        return _entities.Remove(entity);
    }

    public async Task Save()
    {
        await _dbContext.Save();
    }
}