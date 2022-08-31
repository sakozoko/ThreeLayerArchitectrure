using AutoMapper;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Objects;
using BLL.Services.Interfaces;
using DAL;
using Entities;

namespace BLL.Services;

internal sealed class CategoryService : BaseService, ICategoryService
{
    public CategoryService(IUnitOfWork unitOfWork, ITokenHandler tokenHandler, ILogger logger,
        IMapper mapper) : base(unitOfWork, tokenHandler, logger, mapper)
    {
    }


    public Task<int> Create(string token, string name)
    {
        return Task<int>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            if (!ValidateChangeNameOrCreate(requestUser, name)) return -1;
            var category = new Category { Name = name };
            var categoryId = UnitOfWork.CategoryRepository.Add(Mapper.Map<CategoryEntity>(category));
            Logger.Log($"Admin {requestUser.Name} created category with id {categoryId}");
            return categoryId;
        });
    }

    public Task<bool> ChangeName(string token, string newName, int categoryId)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            if (!ValidateChangeNameOrCreate(requestUser, newName)) return false;
            var category = Mapper.Map<Category>(UnitOfWork.CategoryRepository.GetById(categoryId));
            if (category is null) return false;
            category.Name = newName;
            UnitOfWork.CategoryRepository.Update(Mapper.Map<CategoryEntity>(category));
            Logger.Log($"Admin {requestUser.Name} changed name category with id {categoryId}");
            return true;
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

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

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
        return Task<bool>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);

            var targetCategory = UnitOfWork.CategoryRepository.GetById(id);
            if (targetCategory is null) return false;
            Logger.Log($"Admin {requestUser.Name} removed category with id {targetCategory.Id}");
            return UnitOfWork.CategoryRepository.Delete(targetCategory);
        });
    }

    private bool ValidateChangeNameOrCreate(UserEntity requestUser, string name)
    {
        ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);
        if (string.IsNullOrWhiteSpace(name))
            return false;
        var nameIsUnique = UnitOfWork.CategoryRepository.GetAll().FirstOrDefault(x => x.Name == name) == null;
        return nameIsUnique;
    }
}