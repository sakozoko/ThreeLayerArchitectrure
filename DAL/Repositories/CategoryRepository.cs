using DAL.DataContext;
using Entities.Goods;

namespace DAL.Repositories;

public class CategoryRepository : IRepository<Category>
{
    private readonly DbContext _dbContext;
    private readonly List<Category> _categories;
    public CategoryRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _categories = dbContext.Categories;
    }
    
    public void Add(Category entity)
    {
        _categories.Add(entity);
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