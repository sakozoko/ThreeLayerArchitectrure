using DAL.DataContext;
using Entities;

namespace DAL.Repositories;

internal class GenericRepository<T> : SyncRepository, IRepository<T> where T : BaseEntity
{
    private readonly IDbContext _dbContext;

    private int _lastId;

    public GenericRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
        _lastId = _dbContext.Set<T>().Count;
    }

    public int Add(T entity)
    {
        lock (Obj)
        {
            entity.Id = ++_lastId;
            _dbContext.Set<T>().Add(entity);
            return _lastId;
        }
    }

    public IEnumerable<T> GetAll()
    {
        lock (Obj)
        {
            return _dbContext.Set<T>();
        }
    }

    public T GetById(int id)
    {
        lock (Obj)
        {
            return _dbContext.Set<T>().FirstOrDefault(x => x.Id == id);
        }
    }

    public bool Delete(T entity)
    {
        lock (Obj)
        {
            return _dbContext.Set<T>().Remove(entity);
        }
    }

    public void Update(T entity)
    {
        var value = _dbContext.Set<T>().FirstOrDefault(x => x.Id == entity.Id);
        if (value is null) throw new ArgumentException(nameof(entity));

        lock (Obj)
        {
            _dbContext.Set<T>().Remove(value);
            _dbContext.Set<T>().Add(entity);
        }
    }
}