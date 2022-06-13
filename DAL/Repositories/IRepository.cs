namespace DAL.Repositories;

public interface IRepository<T> where T : class
{
    public void Add(T entity);
    public IEnumerable<T> GetAll();
    public T GetById(int id);
    public void Delete(T entity);
    
}