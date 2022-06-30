using BLL.Extension;
using BLL.Helpers.Token;
using BLL.Objects;
using BLL.Services.Interfaces;
using BLL.Util.Logger;
using DAL.Repositories;
using Entities;

namespace BLL.Services;

public class CategoryService : BaseService<CategoryEntity>, ICategoryService
{
    public CategoryService(IRepository<CategoryEntity> repository, ITokenHandler tokenHandler, ILogger logger) : base(
        repository, tokenHandler, logger)
    {
    }


    public Task<int> Create(string token, string name) =>
        Task<int>.Factory.StartNew(() =>
        {
            if (string.IsNullOrWhiteSpace(name))
                return -1;
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);
            var nameIsUnique = Repository.GetAll().FirstOrDefault(x => x.Name == name) == null;
            if (!nameIsUnique) return -1;
            var category = new Objects.Category { Name = name };
            var id = Repository.Add(category.ToEntity());
            Logger.Log($"Admin {requestUser.Name} created category with id {id}");
            return id;
        });

    public Task<IEnumerable<Objects.Category>> GetAll(string token) =>
        Task<IEnumerable<Objects.Category>>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            return Repository.GetAll().ToDomain();
        });

    public Task<Objects.Category> GetByName(string token, string name) =>
        Task<Objects.Category>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);

            Logger.Log($"Admin {requestUser.Name} reviewed category by name {name}");

            return Repository.GetAll().FirstOrDefault(x => x.Name == name).ToDomain();
        });

    public Task<Objects.Category> GetById(string token, int id) =>
        Task<Objects.Category>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            return Repository.GetById(id).ToDomain();
        });

    public Task<bool> Remove(string token, int id) => 
        Remove(token, Repository.GetById(id).ToDomain());

    public Task<bool> Remove(string token, Objects.Category entity) =>
        Task<bool>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);

            return Repository.Delete(entity.ToEntity());
        });
}