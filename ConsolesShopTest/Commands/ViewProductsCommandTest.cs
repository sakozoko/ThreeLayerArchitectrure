using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using BLL.Command;
using DAL.Repositories;
using Entities.Goods;
using Entities.User;
using Moq;
using Xunit;
namespace ConsolesShopTest.Commands;

public class ViewProductsCommandTest
{
    [Fact]
    public void FieldNamesWorksCorrectly()
    {
        //arrange
        var productRepos = new Mock<IRepository<Product>>();
        productRepos.Setup(x => x.GetAll()).Returns(Array.Empty<Product>());
        var command = new ViewProductsCommand(productRepos.Object);
        //act
        var fieldInfoNames = command.GetType().
            GetField("Names", BindingFlags.NonPublic | BindingFlags.Static);
        var fieldInfoBaseTypeNames = command.GetType().BaseType?
            .GetField("_names", BindingFlags.Instance | BindingFlags.NonPublic);
        //assert
        var expected = fieldInfoBaseTypeNames?.GetValue(command);
        var actual = fieldInfoNames?.GetValue(command);
        Assert.Same(expected,actual);
    }
    [Fact]
    public void ExecuteTest()
    {
        Category[] categories = 
        {
            new(1,"First category"),
            new(2,"Second category"),
            new(3,"Third category")
        };
        Product[] products = 
        {
            new (1, "NameOne", "Decription one", 123, categories[0]),
            new (2, "NameTwo", "Decription two", 321, categories[1]),
            new (3, "NameThree", "Decription three", 5123, categories[2]),
            new (4, "NameFour", "Decription four", 6321, categories[2])
        };
        var repos = new Mock<IRepository<Product>>();
        repos.Setup(x => x.GetAll()).Returns(products);
        var command = new ViewProductsCommand(repos.Object);

        // var
        Task<string> commtask = command.Execute();
        commtask.Wait();  
        var str = commtask.Result;
        var actual = true;
        //assert
        //output line should contain pair of the product name and some property
        foreach (var product in products)
        {
            if (!str.Contains(product.Name)
                || !(str.Contains(product.Id.ToString())
                || str.Contains(product.Description)
                || str.Contains(product.Cost.ToString(CultureInfo.InvariantCulture))
                || str.Contains(product.Category.ToString()))) 
                actual = false;
        }

        Assert.True(actual);
    }

    [Fact]
    public void GroupViewTest()
    {
        var repos = new Mock<IRepository<Product>>();
        repos.Setup(x => x.GetAll()).Returns(Array.Empty<Product>());
        var user = new Mock<IUser>();
        var command = new ViewProductsCommand(repos.Object);
        var fieldInfo = command.GetType().GetField("_groupKey", BindingFlags.Instance | BindingFlags.NonPublic);
        command.CanExecute(user.Object, new[] { "-g" });
        command.Execute().Wait();
        var actual = (bool)fieldInfo?.GetValue(command)!;
        Assert.True(actual);
    }
}