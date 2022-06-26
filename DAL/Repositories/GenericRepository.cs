using DAL.DataContext;
using Entities;

namespace DAL.Repositories;

internal class GenericRepository<T> : SyncRepository, IRepository<T> where T : BaseEntity
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
        
        lock (Obj)
        {
            entity.Id = ++_lastId;
            _entities.Add(entity);
            return _lastId;
        }

        
    }

    public IEnumerable<T> GetAll()
    {
        lock (Obj)
        {
            return _entities;
        }
    }

    public T GetById(int id)
    {
        lock (Obj)
        {
            return _entities.FirstOrDefault(x => x.Id == id);
        }
    }

    public bool Delete(T entity)
    {
        lock (Obj)
        {
            return _entities.Remove(entity); 
        }
    }

    public async Task Save()
    {
        await _dbContext.Save();
    }
}