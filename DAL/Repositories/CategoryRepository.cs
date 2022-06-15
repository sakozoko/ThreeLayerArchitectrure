using DAL.DataContext;
using Entities.Goods;

namespace DAL.Repositories;

public class CategoryRepository : IRepository<Category>
{
    private readonly List<Category> _categories;
    private readonly DbContext _dbContext;
    private int _lastId;

    public CategoryRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _categories = dbContext.Categories;
        _lastId = _categories.Count;
    }

    public int Add(Category entity)
    {
        entity.Id = ++_lastId;
        _categories.Add(entity);
        return _lastId;
    }

    public IEnumerable<Category> GetAll()
    {
        return _categories;
    }

    public Category GetById(int id)
    {
        return _categories.Find(x => x.Id == id);
    }

    public void Delete(Category entity)
    {
        _categories.Remove(entity);
    }
}