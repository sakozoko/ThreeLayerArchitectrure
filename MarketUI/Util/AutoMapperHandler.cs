using AutoMapper;
using BLL.Objects;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Util;

public class AutoMapperHandler : IUserInterfaceMapperHandler
{
    public IMapper GetMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserModel>();
            cfg.CreateMap<UserModel, User>();
            cfg.CreateMap<Product, ProductModel>();
            cfg.CreateMap<ProductModel, Product>();
            cfg.CreateMap<Category, CategoryModel>();
            cfg.CreateMap<CategoryModel, Category>();
            cfg.CreateMap<Order, OrderModel>();
            cfg.CreateMap<OrderModel, Order>();
            cfg.CreateMap<AuthenticateRequestModel, AuthenticateRequest>();
            cfg.CreateMap<AuthenticateResponse, AuthenticateResponseModel>();
        });
        return config.CreateMapper();
    }
}