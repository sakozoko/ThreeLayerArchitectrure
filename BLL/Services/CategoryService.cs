using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using BLL.Helpers;
using BLL.Logger;
using BLL.Services.Interfaces;
using DAL.Repositories;
using Entities.Goods;

namespace BLL.Services;

public class CategoryService : BaseService<Category>, ICategoryService
{
    public CategoryService(IRepository<Category> repository, CustomTokenHandler tokenHandler, ILogger logger) : base(repository, tokenHandler, logger)
    {
    }
    

    public Task<int> Create(string token, string name) =>
        Task<int>.Factory.StartNew(() =>
        {
            if (string.IsNullOrWhiteSpace(name))
                return -1;
            var requestUser = TokenHandler.GetUser(token);
            ThrowServiceExceptionIfUserIsNullOrNotAdmin(requestUser);
            var nameIsUnique = Repository.GetAll().FirstOrDefault(x => x.Name == name) == null;
            if (!nameIsUnique) return -1;
            var category = new Category { Name = name };
            var id = Repository.Add(category);
            Logger.Log($"Admin {requestUser.Name} created category with id {id}");
            return id;
        });

    public Task<IEnumerable<Category>> GetAll(string token) =>
        Task<IEnumerable<Category>>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            
            ThrowServiceExceptionIfUserIsNull(requestUser);
            
            return Repository.GetAll();
        });

    public Task<Category> GetByName(string token, string name)=>
        Task<Category>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            
            ThrowServiceExceptionIfUserIsNullOrNotAdmin(requestUser);
            
            Logger.Log($"Admin {requestUser.Name} reviewed category by name {name}");
            
            return Repository.GetAll().FirstOrDefault(x => x.Name == name);
        });

    public Task<Category> GetById(string token, int id)=>
        Task<Category>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            
            ThrowServiceExceptionIfUserIsNull(requestUser);
            
            return Repository.GetById(id);
        });

    public Task<bool> Remove(string token, int id) => 
        Remove(token, Repository.GetById(id));

    public Task<bool> Remove(string token, Category entity)=>
        Task<bool>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            
            ThrowServiceExceptionIfUserIsNullOrNotAdmin(requestUser);
            
            return Repository.Delete(entity);
        });
}