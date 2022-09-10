using System.Threading.Tasks;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Objects;
using BLL.Test.Helpers;
using DAL;
using DAL.Repositories;
using Entities;
using Moq;
using Xunit;

namespace BLL.Test
{
    public class UserServiceTest
    {
        #region AuthenticateMethodTest

        [Fact]
        public void AuthenticateCorrectlyWork()
        {
            const string userName = "UserName";
            const string pass = "newPass";
            var logger = Mock.Of<ILogger>();
            var userRepository = new Mock<IRepository<UserEntity>>();
            userRepository.Setup(x => x.GetAll()).Returns(new[]
            {
                new UserEntity
                {
                    Name = userName, Password = pass
                }
            }).Verifiable();
            var unitOfWork = Mock.Of<IUnitOfWork>(x => x.UserRepository == userRepository.Object);
            var tokenHandler = Mock.Of<ITokenHandler>();
            var service = new ServiceManager(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).UserService;

            var actual = service.Authenticate(new AuthenticateRequest { Name = userName, Password = pass });

            Assert.NotNull(actual);
            userRepository.Verify(x => x.GetAll(), Times.Once);
        }

        #endregion

        #region RegistrationMethodTest

        [Fact]
        public void RegistrationCorrectlyWork()
        {
            const string userName = "UserName";
            const string pass = "newPass";
            var logger = Mock.Of<ILogger>();
            var userRepository = new Mock<IRepository<UserEntity>>();
            userRepository.Setup(x => x.GetAll()).Returns(new[]
            {
                new UserEntity
                {
                    Name = userName + "asd", Password = pass
                }
            }).Verifiable();
            userRepository.Setup(x =>
                    x.Add(It.Is<UserEntity>(c => c.Name == userName && c.Password == pass)))
                .Verifiable();
            var unitOfWork = Mock.Of<IUnitOfWork>(x => x.UserRepository == userRepository.Object);
            var tokenHandler = Mock.Of<ITokenHandler>();
            var service = new ServiceManager(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).UserService;

            var actual = service.Registration(new AuthenticateRequest { Name = userName, Password = pass });

            Assert.NotNull(actual);
            userRepository.Verify(x => x.GetAll(), Times.Once);
            userRepository.Verify(x => x.Add(It.Is<UserEntity>(c => c.Name == userName && c.Password == pass)),
                Times.Once);
        }

        #endregion

        #region GetAuthenticateResponseMethodTest

        [Fact]
        public void GetAuthenticateResponseTestUserIsNullReturnedException()
        {
            ServiceTest.MethodsTestReturnedException<ServiceManager>(null,
                manager => Task.FromResult(manager.UserService.GetAuthenticateResponse("")));
        }

        #endregion

        #region GetByIdMethodTest

        [Fact]
        public void GetByIdCorrectlyWork()
        {
            const int userId = 1;
            var logger = Mock.Of<ILogger>();
            var userRepository = new Mock<IRepository<UserEntity>>();
            userRepository.Setup(x => x.GetById(userId)).Returns(new UserEntity
            {
                Id = 1
            }).Verifiable();
            var unitOfWork = Mock.Of<IUnitOfWork>(x => x.UserRepository == userRepository.Object);
            var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser("") == new UserEntity());
            var service = new ServiceManager(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).UserService;

            var actual = service.GetById("", userId).Result.Id;

            Assert.Equal(userId, actual);
            userRepository.Verify(x => x.GetById(userId), Times.Once);
        }

        [Fact]
        public void GetByIdTestUserIsNullReturnedException()
        {
            ServiceTest.MethodsTestReturnedException<ServiceManager>(null,
                manager => manager.UserService.GetById("", 1));
        }

        #endregion

        #region ChangePasswordMethodTest

        [Fact]
        public void ChangePasswordCorrectlyWork()
        {
            const int userId = 1;
            const string oldPass = "oldPassword";
            const string newPass = "newPassword_:";
            var logger = Mock.Of<ILogger>();
            var userRepository = Mock.Of<IRepository<UserEntity>>();
            var unitOfWork = Mock.Of<IUnitOfWork>(x => x.UserRepository == userRepository);
            var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser("") == new UserEntity
            {
                Id = userId,
                Password = oldPass
            });
            var service = new ServiceManager(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).UserService;

            var actual = service.ChangePassword("", newPass, oldPass).Result;

            Assert.True(actual);
        }

        [Fact]
        public void ChangePasswordTestUserIsNullReturnedException()
        {
            ServiceTest.MethodsTestReturnedException<ServiceManager>(null,
                manager => manager.UserService.ChangePassword("", "", "", 1));
        }

        [Fact]
        public void ChangePasswordTestUserIsNotAdminReturnedException()
        {
            ServiceTest.MethodsTestReturnedException<ServiceManager>(new UserEntity(),
                manager => manager.UserService.ChangePassword("", "", "", 1));
        }

        #endregion

        #region ChangeNameMethodTest

        [Fact]
        public void ChangeNameCorrectlyWork()
        {
            const string newName = "newName";
            var logger = Mock.Of<ILogger>();
            var userRepository = new Mock<IRepository<UserEntity>>();
            userRepository.Setup(x => x.GetAll()).Returns(new[] { new UserEntity { Name = "asdas" } }).Verifiable();
            var unitOfWork = Mock.Of<IUnitOfWork>(x => x.UserRepository == userRepository.Object);
            var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser("") == new UserEntity());
            var service = new ServiceManager(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).UserService;

            var actual = service.ChangeName("", newName).Result;

            Assert.True(actual);
            userRepository.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public void ChangeNameTestUserIsNullReturnedException()
        {
            ServiceTest.MethodsTestReturnedException<ServiceManager>(null,
                manager => manager.UserService.ChangeName("", "", 1));
        }

        [Fact]
        public void ChangeNameTestUserIsNotAdminReturnedException()
        {
            ServiceTest.MethodsTestReturnedException<ServiceManager>(new UserEntity(),
                manager => manager.UserService.ChangeName("", "", 1));
        }

        #endregion

        #region ChangeSurnameMethodTest

        [Fact]
        public void ChangeSurNameCorrectlyWork()
        {
            const string newSurname = "newSurName";
            var logger = Mock.Of<ILogger>();
            var userRepository = new Mock<IRepository<UserEntity>>();
            var unitOfWork = Mock.Of<IUnitOfWork>(x => x.UserRepository == userRepository.Object);
            var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser("") == new UserEntity());
            var service = new ServiceManager(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).UserService;

            var actual = service.ChangeName("", newSurname).Result;

            Assert.True(actual);
        }

        [Fact]
        public void ChangeSurnameTestUserIsNullReturnedException()
        {
            ServiceTest.MethodsTestReturnedException<ServiceManager>(null,
                manager => manager.UserService.ChangeSurname("", "", 1));
        }

        [Fact]
        public void ChangeSurnameTestUserIsNotAdminReturnedException()
        {
            ServiceTest.MethodsTestReturnedException<ServiceManager>(new UserEntity(),
                manager => manager.UserService.ChangeSurname("", "", 1));
        }

        #endregion
    }
}