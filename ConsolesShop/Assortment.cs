using System.Collections.Generic;
using ConsolesShop.Goods;
using ConsolesShop.User;

namespace ConsolesShop;

public static class Assortment
{
    public static readonly List<Category> Categories = new()
    {
        new Category("First"),
        new Category("Second"),
        new Category("Third"),
        new Category("Fourth"),
        new Category("Fifth")
    };

    public static readonly List<Product> Products = new()
    {
        new Product(1, "First", "First description", 12, Categories[0]),
        new Product(2, "Second", "Second description", 354, Categories[2]),
        new Product(3, "Third", "Third description", 6364, Categories[3]),
        new Product(4, "Fourth", "Fourth description", 1454, Categories[1]),
        new Product(5, "Fifth", "Fifth description", 561, Categories[4])
    };

    public static readonly IUser[] Users =
    {
        new Administrator(1,"Admin", "", "123123"),
        new RegisteredUser(2,"Alex", "John", "332211")
    };
}