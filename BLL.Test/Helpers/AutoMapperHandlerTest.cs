using AutoMapper;
using BLL.Objects;
using BLL.Util.Interface;
using Entities;

namespace BLL.Test.Helpers;

public class AutoMapperHandlerTest : IDomainMapperHandler
{
    public  IMapper GetMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Category, CategoryEntity>();
            cfg.CreateMap<CategoryEntity, Category>();
            cfg.CreateMap<Product, ProductEntity>();
            cfg.CreateMap<ProductEntity, Product>();
            cfg.CreateMap<User, UserEntity>();
            cfg.CreateMap<UserEntity, User>();
            cfg.CreateMap<Order, OrderEntity>();
            cfg.CreateMap<OrderEntity, Order>();
        });
        return config.CreateMapper();
    }
}