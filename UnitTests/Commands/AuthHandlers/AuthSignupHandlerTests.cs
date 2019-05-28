using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using Domain.DBEntities;
using Domain.ModelEntities;
using EICProjectBackend.Handlers;
using EICProjectBackend.Services;
using Moq;
using UnitOfWork;
using Xunit;

namespace UnitTests.Commands.AuthHandlers
{
    public class AuthSignupHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfUnitMock;
        private readonly Mock<IAuthService> _authSearviceMock;

        public AuthSignupHandlerTests()
        {
            this._unitOfUnitMock = new Mock<IUnitOfWork>();
            this._authSearviceMock = new Mock<IAuthService>();
        }

        [Fact]
        public async void Handler_CouldNotCreateUser_Test()
        {
            User userNull = null;
            User user = new User(){Password = "test"};
            this._authSearviceMock.Setup(mock => mock.CalculateHash(It.IsAny<string>())).Returns("testHash");
            this._unitOfUnitMock.Setup(mock => mock.UserRepository.Create(It.IsAny<User>())).Returns(userNull);

            AuthSignupCommand command = new AuthSignupCommand(new AuthSignupModel(){Role = new Role(){Id = 1}});
            AuthSignupHandler handler = new AuthSignupHandler(this._authSearviceMock.Object, this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void Handler_UserCreated_Test()
        {
            User userNull = null;
            User user = new User() { Password = "test" };
            this._authSearviceMock.Setup(mock => mock.CalculateHash(It.IsAny<string>())).Returns("testHash");
            this._unitOfUnitMock.Setup(mock => mock.UserRepository.Create(It.IsAny<User>())).Returns(user);

            AuthSignupCommand command = new AuthSignupCommand(new AuthSignupModel() { Role = new Role() { Id = 1 } });
            AuthSignupHandler handler = new AuthSignupHandler(this._authSearviceMock.Object, this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.True((bool)result);
        }

        [Fact]
        public async void Handler_Exception_Test()
        {
            User userNull = null;
            User user = new User() { Password = "test" };
            this._authSearviceMock.Setup(mock => mock.CalculateHash(It.IsAny<string>())).Returns("testHash");

            var exception = new Exception();           
            
            this._unitOfUnitMock.Setup(mock => mock.UserRepository.Create(It.IsAny<User>()))
                .Throws(exception);

            AuthSignupCommand command = new AuthSignupCommand(new AuthSignupModel() { Role = new Role() { Id = 1 } });
            AuthSignupHandler handler = new AuthSignupHandler(this._authSearviceMock.Object, this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

       
    }
}
