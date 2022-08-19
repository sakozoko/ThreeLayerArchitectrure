using AutoMapper;
using BLL.Helpers.Token;
using BLL.Objects;
using BLL.Services.Interfaces;
using BLL.Util.Logger;
using DAL;
using DAL.Repositories;
using Entities;

namespace BLL.Services;

public class ProductService : BaseService, IProductService
{
    public ProductService(IUnitOfWork unitOfWork, ITokenHandler tokenHandler, ILogger logger,
        IMapper mapper) : base(unitOfWork, tokenHandler, logger, mapper)
    {
    }


    public Task<IEnumerable<Product>> GetByName(string name)
    {
        return Task<IEnumerable<Product>>.Factory.StartNew(() =>
            Mapper.Map<IEnumerable<Product>>(UnitOfWork.ProductRepository.GetAll()).Where(x => x.Name == name));
    }

    public Task<Product> GetById(string token, int id)
    {
        return Task<Product>.Factory.StartNew(() =>
        {
            ThrowAuthenticationExceptionIfUserIsNull(TokenHandler.GetUser(token));
            return Mapper.Map<Product>(UnitOfWork.ProductRepository.GetById(id));
        });
    }

    public Task<IEnumerable<Product>> GetAll(string token)
    {
        return Task<IEnumerable<Product>>.Factory.StartNew(
            () =>
            {
                ThrowAuthenticationExceptionIfUserIsNull(TokenHandler.GetUser(token));
                return Mapper.Map<IEnumerable<Product>>(UnitOfWork.ProductRepository.GetAll());
            });
    }

    public Task<bool> ChangeName(string token, string value, Product product)
    {
        return Task<bool>.Factory.StartNew(() => !string.IsNullOrWhiteSpace(value) && ChangeProperty(token, x => x.Name = value, product));
    }

    public Task<bool> ChangeDescription(string token, string value, Product product)
    {
        return Task<bool>.Factory.StartNew(() => !string.IsNullOrWhiteSpace(value) && ChangeProperty(token, x => x.Description = value, product));
    }

    public Task<bool> ChangeCost(string token, decimal value, Product product)
    {
        return Task<bool>.Factory.StartNew(() => value>0 && ChangeProperty(token, x => x.Cost = value, product));
    }

    public Task<bool> ChangeCategory(string token, Category category, Product product)
    {
        return Task<bool>.Factory.StartNew(() =>
            category is not null && ChangeProperty(token, x => x.Category = category, product));
    }

    public Task<bool> Remove(string token, Product product)
    {
        return Task<bool>.Factory.StartNew(() => product != null && Remove(token, product.Id).Result);
    }

    public Task<bool> Remove(string token, int id)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);
            Logger.Log($"Admin {requestUser.Name} invoked remove product with id {id}");
            return UnitOfWork.ProductRepository.Delete(UnitOfWork.ProductRepository.GetById(id));
        });
    }

    public Task<IEnumerable<Product>> GetFromOrder(string token, Order order)
    {
        return Task<IEnumerable<Product>>.Factory.StartNew(() =>
        {
            if (order is null)
                return null;
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            if (order.Owner.Id != requestUser.Id)
            {
                ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
                Logger.Log($"Admin {requestUser.Name} invoked to get products from order id {order.Id}");
            }

            return order.Products;
        });
    }

    private bool ChangeProperty(string token, Action<Product> act, Product product)
    {
        if (product is null)
            return false;
        var requestUser = TokenHandler.GetUser(token);

        ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);

        act.Invoke(product);
        Logger.Log($"Admin {requestUser.Name} changed property for product id {product.Id}");
        UnitOfWork.ProductRepository.Update(Mapper.Map<ProductEntity>(product));
        return true;
    }
}