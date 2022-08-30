using BLL.Objects;

namespace BLL.Services.Interfaces;

public interface ICategoryService
{
    Task<int> Create(string token, string name);
    Task<IEnumerable<Category>> GetAll(string token);
    Task<Category> GetByName(string token, string name);

    Task<bool> ChangeName(string token, string newName, int categoryId);
    Task<Category> GetById(string token, int id);
    Task<bool> Remove(string token, int id);
}