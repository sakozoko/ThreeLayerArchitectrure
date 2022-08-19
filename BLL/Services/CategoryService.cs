using AutoMapper;
using BLL.Helpers.Token;
using BLL.Objects;
using BLL.Services.Interfaces;
using BLL.Util.Logger;
using DAL;
using DAL.Repositories;
using Entities;

namespace BLL.Services;

public class CategoryService : BaseService, ICategoryService
{
    public CategoryService(IUnitOfWork unitOfWork, ITokenHandler tokenHandler, ILogger logger,
        IMapper mapper) : base(unitOfWork, tokenHandler, logger, mapper)
    {
    }


    public Task<int> Create(string token, string name)
    {
        return Task<int>.Factory.StartNew(() =>
        {
            if (string.IsNullOrWhiteSpace(name))
                return -1;
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);
            var nameIsUnique = UnitOfWork.CategoryRepository.GetAll().FirstOrDefault(x => x.Name == name) == null;
            if (!nameIsUnique) return -1;
            var category = new Category { Name = name };
            var id = UnitOfWork.CategoryRepository.Add(Mapper.Map<CategoryEntity>(category));
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

            return Mapper.Map<IEnumerable<Category>>(UnitOfWork.CategoryRepository.GetAll());
        });
    }

    public Task<Category> GetByName(string token, string name)
    {
        return Task<Category>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);

            Logger.Log($"Admin {requestUser.Name} reviewed category by name {name}");

            return Mapper.Map<Category>(UnitOfWork.CategoryRepository.GetAll().FirstOrDefault(x => x.Name == name));
        });
    }

    public Task<Category> GetById(string token, int id)
    {
        return Task<Category>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            return Mapper.Map<Category>(UnitOfWork.CategoryRepository.GetById(id));
        });
    }

    public Task<bool> Remove(string token, int id)
    {
        return Remove(token, Mapper.Map<Category>(UnitOfWork.CategoryRepository.GetById(id)));
    }

    public Task<bool> Remove(string token, Category category)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);

            return UnitOfWork.CategoryRepository.Delete(Mapper.Map<CategoryEntity>(category));
        });
    }
}