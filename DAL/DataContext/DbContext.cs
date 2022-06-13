
using Entities;
using Entities.Goods;
using Entities.User;

namespace DAL.DataContext;

public class DbContext
{    
    public readonly List<Category> Categories = new()
    {
        new Category(1,"First"),
        new Category(2,"Second"),
        new Category(3,"Third"),
        new Category(4,"Fourth"),
        new Category(5,"Fifth")
    };
    public  readonly List<IUser> Users =new()
    {
        new Administrator(1, "Admin", "", "123123"),
        new RegisteredUser(2, "Alex", "John", "332211")
    };
    public readonly List<Product> Products;
    public readonly List<Order> Orders;
    public DbContext()
    {
        Products= new List<Product>
        {
            new (1, "First", "First description", 12, Categories[0]),
            new (2, "Second", "Second description", 354, Categories[2]),
            new (3, "Third", "Third description", 6364, Categories[3]),
            new (4, "Fourth", "Fourth description", 1454, Categories[0]),
            new (5, "Fifth", "Fifth description", 561, Categories[4])
        };
        Orders = new List<Order>
        {
            new (1, "Vishneva st. 34", Users[1]),
            new (2, "Vishneva st. 34", Users[1]),
            new (3, "Vishneva st. 34", Users[1]),
            new (4, "Vishneva st. 34", Users[1]),
            new (5, "Vishneva st. 34", Users[1]),
            new (6, "Vishneva st. 34", Users[1])
        };

        foreach (var user in Users)
        {
            foreach (var order in Orders.Where(order => order.Owner.Equals(user)))
            {
                user.Orders.Add(order);
            }
        }
    }








}