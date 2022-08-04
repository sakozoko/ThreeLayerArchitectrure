using AutoMapper;
using BLL.Extension;
using BLL.Helpers.Token;
using BLL.Objects;
using BLL.Services.Interfaces;
using BLL.Util.Interface;
using BLL.Util.Logger;
using DAL.Repositories;
using Entities;

namespace BLL.Services;

public class CategoryService : BaseService<CategoryEntity>, ICategoryService
{



    public Task<int> Create(string token, string name)
    {
        return Task<int>.Factory.StartNew(() =>
        {
            if (string.IsNullOrWhiteSpace(name))
                return -1;
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);
            var nameIsUnique = Repository.GetAll().FirstOrDefault(x => x.Name == name) == null;
            if (!nameIsUnique) return -1;
            var category = new Category { Name = name };
            var id = Repository.Add(Mapper.Map<CategoryEntity>(category));
            Logger.Log($"Admin {requestUser.Name} created category with id {id}");
            return id;
        });
    }

    public Task<IEnumerable<Category>> GetAll(string token)
    {
        return Task<IEnumerable<Category>>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            return Mapper.Map<IEnumerable<Category>>(Repository.GetAll());
        });
    }

    public Task<Category> GetByName(string token, string name)
    {
        return Task<Category>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);

            Logger.Log($"Admin {requestUser.Name} reviewed category by name {name}");

            return Mapper.Map<Category>(Repository.GetAll().FirstOrDefault(x => x.Name == name));
        });
    }

    public Task<Category> GetById(string token, int id)
    {
        return Task<Category>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            return Mapper.Map<Category>(Repository.GetById(id));
        });
    }

    public Task<bool> Remove(string token, int id)
    {
        return Remove(token, Mapper.Map<Category>(Repository.GetById(id)));
    }

    public Task<bool> Remove(string token, Category category)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);

            return Repository.Delete(Mapper.Map<CategoryEntity>(category));
        });
    }

    public CategoryService(IRepository<CategoryEntity> repository, ITokenHandler tokenHandler, ILogger logger, IMapper mapper) : base(repository, tokenHandler, logger, mapper)
    {
    }
}