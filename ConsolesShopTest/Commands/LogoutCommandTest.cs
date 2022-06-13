using BLL.Command;
using Entities.User;
using Moq;
using Xunit;

namespace ConsolesShopTest.Commands;

public class LogoutCommandTest
{
    [Fact]
    public void LogoutSuccessful()
    {
        //arrange
        var user = new Mock<IUser>();
        user.SetupGet(x => x.IsLoggedIn).Returns(true).Verifiable();
        var actual = false;
        var command = new LogoutCommand(() => { actual = true; });
        //act
        if (command.CanExecute(user.Object))
            command.Execute().Wait();
        else
            Assert.True(command.CanExecute(user.Object));
        //assert
        Assert.True(actual);
        user.Verify(x => x.IsLoggedIn, Times.Once());
    }

    [Fact]
    public void LogoutFailedWhenIsLoggedInEqualsFalse()
    {
        //arrange
        var user = new Mock<IUser>();
        user.SetupGet(x => x.IsLoggedIn).Returns(false).Verifiable();
        var command = new LogoutCommand(() => { });
        //act
        var actual = command.CanExecute(user.Object);
        //assert
        Assert.False(actual);
        user.Verify(x => x.IsLoggedIn, Times.Once());
    }

    [Fact]
    public void LogoutFailedWhenUserIsNull()
    {
        //arrange
        var command = new LogoutCommand(() => { });
        //act
        var actual = command.CanExecute(null);
        //assert
        Assert.False(actual);
    }
}