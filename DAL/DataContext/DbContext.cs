using Entities;
using Entities.Goods;

namespace DAL.DataContext;

public class DbContext
{
    public readonly List<Category> Categories = new()
    {
        new Category { Id = 1, Name = "First" },
        new Category { Id = 2, Name = "Second" },
        new Category { Id = 3, Name = "Third" },
        new Category { Id = 4, Name = "Fourth" },
        new Category { Id = 5, Name = "Fifth" }
    };

    public readonly List<Order> Orders;
    public readonly List<Product> Products;

    public readonly List<User> Users = new()
    {
        new User { Id = 1, Name = "Admin", IsAdmin = true, Password = "123123" },
        new User { Id = 2, Name = "Alex", Surname = "John", IsAdmin = false, Password = "332211" }
    };

    public DbContext()
    {
        Products = new List<Product>
        {
            new(1, "First", "First description", 12, Categories[0]),
            new(2, "Second", "Second description", 354, Categories[2]),
            new(3, "Third", "Third description", 6364, Categories[3]),
            new(4, "Fourth", "Fourth description", 1454, Categories[0]),
            new(5, "Fifth", "Fifth description", 561, Categories[4])
        };
        Orders = new List<Order>
        {
            new() { Id = 1, Description = "Vishneva st. 34", Owner = Users[1] },
            new() { Id = 2, Description = "Vishneva st. 34", Owner = Users[1] },
            new() { Id = 3, Description = "Vishneva st. 34", Owner = Users[1] },
            new() { Id = 4, Description = "Vishneva st. 34", Owner = Users[1] },
            new() { Id = 5, Description = "Vishneva st. 34", Owner = Users[1] },
            new() { Id = 6, Description = "Vishneva st. 34", Owner = Users[1] }
        };
    }
}