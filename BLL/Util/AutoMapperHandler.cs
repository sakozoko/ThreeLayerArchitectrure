using AutoMapper;
using BLL.Objects;
using BLL.Util.Interface;
using Entities;

namespace BLL.Util;

public class AutoMapperHandler : IDomainMapperHandler
{
    public IMapper GetMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Category, CategoryEntity>();
            cfg.CreateMap<CategoryEntity, Category>();
            cfg.CreateMap<Product, ProductEntity>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
            cfg.CreateMap<ProductEntity, Product>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
            cfg.CreateMap<User, UserEntity>();
            cfg.CreateMap<UserEntity, User>();
            cfg.CreateMap<Order, OrderEntity>().ForMember(dest => dest.Owner,
                    opt => opt.MapFrom(src => src.Owner))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
            cfg.CreateMap<OrderEntity, Order>().ForMember(dest => dest.Owner,
                opt => opt.MapFrom(src => src.Owner)).ForMember(dest => dest.Products,
                opt => opt.MapFrom(src => src.Products));
        });
        return config.CreateMapper();
    }
}