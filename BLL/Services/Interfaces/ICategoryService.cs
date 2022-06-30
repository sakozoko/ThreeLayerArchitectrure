using BLL.Objects;

namespace BLL.Services.Interfaces;

public interface ICategoryService
{
    public Task<int> Create(string token, string name);
    public Task<IEnumerable<Category>> GetAll(string token);
    public Task<Category> GetByName(string token, string name);
    public Task<Category> GetById(string token, int id);
    public Task<bool> Remove(string token, int id);
    public Task<bool> Remove(string token, Category entity);
}