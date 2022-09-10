using System;
using System.Threading.Tasks;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Services.Exception;
using DAL;
using Entities;
using Moq;
using Xunit;

namespace BLL.Test.Helpers
{
    public static class ServiceTest
    {
        public static void MethodsTestReturnedException<T>(UserEntity tokenHandlerReturnedValue,
            Func<T, Task> testMethod) where T : IServiceManager
        {
            var unitOfWork = Mock.Of<IUnitOfWork>();
            MethodsTestReturnedException(tokenHandlerReturnedValue, unitOfWork, testMethod);
        }

        public static void MethodsTestReturnedException<T>(UserEntity tokenHandlerReturnedValue, IUnitOfWork unitOfWork,
            Func<T, Task> testMethod) where T : IServiceManager
        {
            var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == tokenHandlerReturnedValue);
            var moqLogger = new Mock<ILogger>();
            moqLogger.Setup(x => x.Log(It.IsAny<Exception>())).Verifiable();
            moqLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();

            var service = (T)Activator.CreateInstance(typeof(T), unitOfWork, moqLogger.Object, tokenHandler,
                new AutoMapperHandlerTest());

            Assert.ThrowsAsync<AuthenticationException>(() => testMethod.Invoke(service)).Wait();
            moqLogger.Verify(x => x.Log(It.IsAny<Exception>()), Times.Once);
            moqLogger.Verify(x => x.Log(It.IsAny<string>()), Times.Never);
        }
    }
}