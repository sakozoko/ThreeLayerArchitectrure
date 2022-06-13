using BLL.Command;
using DAL.Repositories;
using Entities.User;
using Moq;
using Xunit;

namespace ConsolesShopTest.Commands;

public class LoginCommandTest
{
    [Fact]
    public void LoginLogicWorkingCorrectlyOne()
    {
        //arrange
        var adm = new Mock<IUser>();
        adm.Setup(x => x.Name).Returns("FirstName").Verifiable();
        adm.Setup(x => x.Login("123")).Verifiable();
        var reg = new Mock<IUser>();
        reg.Setup(x => x.Name).Returns("SecondName").Verifiable();
        var repos = new Mock<IRepository<IUser>>();
        repos.Setup(x => x.GetAll()).Returns(new[] { adm.Object, reg.Object });
        var command = new LoginCommand(repos.Object, _ => { });
        command.CanExecute(null, new[]
        {
            "-n",
            "FirstName",
            "-p",
            "123"
        });
        //act
        command.Execute().Wait();
        //assert
        adm.Verify(x => x.Name, Times.Once);
        adm.Verify(x => x.Login("123"), Times.Once);
        reg.Verify(x => x.Name, Times.Never);
    }

    [Fact]
    public void LoginLogicWorkingCorrectlyTwo()
    {
        //arrange
        var adm = new Mock<IUser>();
        adm.Setup(x => x.Name).Returns("FirstName").Verifiable();
        adm.Setup(x => x.Login("123")).Verifiable();
        var reg = new Mock<IUser>();
        reg.Setup(x => x.Name).Returns("SecondName").Verifiable();
        var repos = new Mock<IRepository<IUser>>();
        repos.Setup(x => x.GetAll()).Returns(new[] { adm.Object, reg.Object });
        var command = new LoginCommand(repos.Object, _ => { });
        command.CanExecute(null, new[]
        {
            "-n",
            "FirstName",
            "-p",
            "123"
        });
        //act
        command.Execute().Wait();
        //assert
        adm.Verify(x => x.Name, Times.Once);
        adm.Verify(x => x.Login("123"), Times.Once);
    }

    [Theory]
    [InlineData("name", "1523", new[] { "-n", "name", "-p", "1523" }, true)]
    [InlineData("names", "1523", new[] { "-n", "name", "-p", "1523" }, false)]
    [InlineData("namee", "1522", new[] { "-n", "name", "-p", "1523" }, false)]
    [InlineData("Name with white space", "1523", new[] { "-n", "Name with white space", "-p", "1523" }, true)]
    [InlineData("name", "1523 ksjd", new[] { "-n", "name", "-p", "1523 ksjd" }, true)]
    [InlineData("name", "1523 ksjd", new[] { "-n", "name", "-p", "1523ksjd" }, false)]
    public void LoginLogicWorkingCorrectlyWithDifferentParameters(
        string userName, string password, string[] args, bool result)
    {
        //arrange
        var adm = new Mock<IUser>();
        adm.Setup(x => x.Name).Returns(userName);
        adm.Setup(x => x.Login(password)).Returns(true);
        IUser? actual = null;
        var repos = new Mock<IRepository<IUser>>();
        repos.Setup(x => x.GetAll()).Returns(new[] { adm.Object });
        var command = new LoginCommand(repos.Object, x => actual = x);
        command.CanExecute(null, args);
        //act
        command.Execute().Wait();
        //assert
        Assert.Equal(result, adm.Object.Equals(actual));
    }

    [Fact]
    public void LoginSuccessful()
    {
        //arrange
        var adm = new Mock<IUser>();
        adm.Setup(x => x.Name).Returns("FirstName");
        adm.Setup(x => x.Login("123")).Returns(true);
        IUser? actual = null;
        var repos = new Mock<IRepository<IUser>>();
        repos.Setup(x => x.GetAll()).Returns(new[] { adm.Object });
        var command = new LoginCommand(repos.Object, x => actual = x);
        command.CanExecute(null, new[]
        {
            "-n",
            "FirstName",
            "-p",
            "123"
        });
        //act
        command.Execute().Wait();
        //assert
        Assert.Same(adm.Object, actual);
    }

    [Fact]
    public void LoginFailedPasswordIncorrect()
    {
        //arrange
        var adm = new Mock<IUser>();
        adm.Setup(x => x.Name).Returns("FirstName");
        adm.Setup(x => x.Login("123")).Returns(true);
        IUser? actual = null;
        var repos = new Mock<IRepository<IUser>>();
        repos.Setup(x => x.GetAll()).Returns(new[] { adm.Object });
        var command = new LoginCommand(repos.Object, x => actual = x);
        command.CanExecute(null, new[]
        {
            "-n",
            "FirstName",
            "-p",
            "123"
        });
        //act
        command.Execute().Wait();
        //assert
        Assert.Same(adm.Object, actual);
    }
}