using Entities;

namespace DAL.Util.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    public int Add(T entity);
    public IEnumerable<T> GetAll();
    public T GetById(int id);
    public bool Delete(T entity);
    public Task Save();
}