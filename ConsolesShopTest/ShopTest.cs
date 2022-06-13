using BLL.Command;
using Entities.User;
using Moq;
using Xunit;

namespace ConsolesShopTest;

public class ShopTest
{
    [Fact]
    public void ExecuteCommandWorksCorrectlyWhenCanExecuteReturnedTrue()
    {
        //arrange
        var user = new Mock<IUser>();
        user.SetupAllProperties();
        var command = new Mock<ICommand>();
        command.Setup(x => x.ItsMe("")).Returns(true).Verifiable();
        command.Setup(x => x.CanExecute(user.Object, null)).Returns(true).Verifiable();
        command.Setup(x => x.Execute(null)).Verifiable();
        var shop = new BLL.Shop(new[] { command.Object }, user.Object);
        //act
        shop.ExecuteCommand("");
        //assert
        command.Verify();
    }

    [Fact]
    public void ExecuteCommandWorksCorrectlyWhenCanExecuteReturnedFalse()
    {
        //arrange
        var user = new Mock<IUser>();
        user.SetupAllProperties();
        var command = new Mock<ICommand>();
        command.Setup(x => x.ItsMe("")).Returns(true);
        command.Setup(x => x.CanExecute(user.Object, null)).Returns(false).Verifiable();
        command.Setup(x => x.Execute(null)).Verifiable();
        var shop = new BLL.Shop(new[] { command.Object }, user.Object);
        //act
        shop.ExecuteCommand("");
        //assert
        command.Verify(x => x.Execute(null), Times.Never);
    }
}