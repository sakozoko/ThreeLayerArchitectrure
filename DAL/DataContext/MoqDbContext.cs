using Entities;
using Moq;

namespace DAL.DataContext;

public class MoqDbContext
{
    public MoqDbContext()
    {
        var users = new List<UserEntity>
        {
            new() { Id = 1, Name = "Admin", IsAdmin = true, Password = "123123" },
            new() { Id = 2, Name = "Alex", Surname = "John", IsAdmin = false, Password = "332211" }
        };
        var categories = new List<CategoryEntity>
        {
            new() { Id = 1, Name = "First" },
            new() { Id = 2, Name = "Second" },
            new() { Id = 3, Name = "Third" },
            new() { Id = 4, Name = "Fourth" },
            new() { Id = 5, Name = "Fifth" }
        };
        var orders = new List<OrderEntity>
        {
            new()
            {
                Id = 1, Description = "Vishneva st. 34", Owner = users[1], Products = new List<ProductEntity>(),
                OrderStatus = "New", Confirmed = true
            },
            new()
            {
                Id = 2, Description = "Vishneva st. 34", Owner = users[1], Products = new List<ProductEntity>(),
                OrderStatus = "CanceledByTheAdministrator", Confirmed = true
            },
            new()
            {
                Id = 3, Description = "Vishneva st. 34", Owner = users[1], Products = new List<ProductEntity>(),
                OrderStatus = "CanceledByTheAdministrator", Confirmed = true
            },
            new()
            {
                Id = 4, Description = "Vishneva st. 34", Owner = users[1], Products = new List<ProductEntity>(),
                OrderStatus = "CanceledByTheAdministrator", Confirmed = true
            },
            new()
            {
                Id = 5, Description = "Vishneva st. 34", Owner = users[1], Products = new List<ProductEntity>(),
                OrderStatus = "Sent", Confirmed = true
            },
            new()
            {
                Id = 6, Description = "Vishneva st. 34", Owner = users[1], Products = new List<ProductEntity>(),
                OrderStatus = "Received", Confirmed = true
            }
        };
        var products = new List<ProductEntity>
        {
            new() { Id = 1, Name = "First", Description = "First description", Cost = 12, Category = categories[0] },
            new() { Id = 2, Name = "Second", Description = "Second description", Cost = 354, Category = categories[2] },
            new() { Id = 3, Name = "Third", Description = "Third description", Cost = 2541, Category = categories[3] },
            new()
            {
                Id = 4, Name = "Fourth", Description = "Fourth description", Cost = 1231, Category = categories[0]
            },
            new() { Id = 5, Name = "Fifth", Description = "Fifth description", Cost = 511, Category = categories[4] }
        };
        MoqDb = Mock.Of<IDbContext>(x =>
            x.Set<OrderEntity>() == orders &&
            x.Set<ProductEntity>() == products &&
            x.Set<UserEntity>() == users &&
            x.Set<CategoryEntity>() == categories &&
            x.Save() == Task.CompletedTask);
    }

    public IDbContext MoqDb { get; }
}