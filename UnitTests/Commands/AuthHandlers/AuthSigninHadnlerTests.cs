using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities;
using Domain.ModelEntities;
using EICProjectBackend.Handlers;
using EICProjectBackend.Services;
using Moq;
using Services;
using UnitOfWork;
using Xunit;

namespace UnitTests.Commands.AuthHandlers
{
    public class AuthSigninHadnlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfUnitMock;
        private readonly Mock<IAuthService> _authSearviceMock;

        public AuthSigninHadnlerTests()
        {
            this._unitOfUnitMock = new Mock<IUnitOfWork>();
            this._authSearviceMock = new Mock<IAuthService>();
        }

        [Fact]
        public async void Handle_False_Test()
        {
            User test = null;
            this._unitOfUnitMock.Setup(mock => mock.UserRepository.ReadViaUserName(It.IsAny<string>())).Returns(Task.FromResult(test));
            
            AuthSigninCommand command = new AuthSigninCommand(new AuthSigninModel());
            AuthSigninHandler handler = new AuthSigninHandler(this._authSearviceMock.Object, this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void Handle_False2_test()
        {
            User test = new User();
            this._unitOfUnitMock.Setup(mock => mock.UserRepository.ReadViaUserName(It.IsAny<string>()))
                .Returns(Task.FromResult(test));


            this._authSearviceMock.Setup(mock => mock.CheckMatch(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            AuthSigninCommand command = new AuthSigninCommand(new AuthSigninModel());
            AuthSigninHandler handler = new AuthSigninHandler(this._authSearviceMock.Object, this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void Handle_False3_test()
        {
            User test = new User();
            this._unitOfUnitMock.Setup(mock => mock.UserRepository.ReadViaUserName(It.IsAny<string>()))
                .Returns(Task.FromResult(test));


            this._authSearviceMock.Setup(mock => mock.CheckMatch(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            Role role = null;
            this._unitOfUnitMock.Setup(mock => mock.UserRepository.ReadUserRole(It.IsAny<int>())).Returns(Task.FromResult(role));

            AuthSigninCommand command = new AuthSigninCommand(new AuthSigninModel());
            AuthSigninHandler handler = new AuthSigninHandler(this._authSearviceMock.Object, this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void Handle_False4_test()
        {
            User test = new User();
            this._unitOfUnitMock.Setup(mock => mock.UserRepository.ReadViaUserName(It.IsAny<string>()))
                .Returns(Task.FromResult(test));


            this._authSearviceMock.Setup(mock => mock.CheckMatch(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            Role role = new Role(){RoleName = "test"};
            this._unitOfUnitMock.Setup(mock => mock.UserRepository.ReadUserRole(It.IsAny<int>())).Returns(Task.FromResult(role));

            this._authSearviceMock.Setup(mock => mock.CreateToken(It.IsAny<User>(), It.IsAny<string>())).Returns("");

            AuthSigninCommand command = new AuthSigninCommand(new AuthSigninModel());
            AuthSigninHandler handler = new AuthSigninHandler(this._authSearviceMock.Object, this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void Handle_Done_test()
        {
            User test = new User();
            this._unitOfUnitMock.Setup(mock => mock.UserRepository.ReadViaUserName(It.IsAny<string>()))
                .Returns(Task.FromResult(test));


            this._authSearviceMock.Setup(mock => mock.CheckMatch(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            Role role = new Role() { RoleName = "test" };
            this._unitOfUnitMock.Setup(mock => mock.UserRepository.ReadUserRole(It.IsAny<int>())).Returns(Task.FromResult(role));

            this._authSearviceMock.Setup(mock => mock.CreateToken(It.IsAny<User>(), It.IsAny<string>())).Returns("test");

            AuthSigninCommand command = new AuthSigninCommand(new AuthSigninModel());
            AuthSigninHandler handler = new AuthSigninHandler(this._authSearviceMock.Object, this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Equal("test", result);
        }

        [Fact]
        public async void Handle_Exception_test()
        {
            User test = new User();
            var error = new Exception();
            this._unitOfUnitMock.Setup(mock => mock.UserRepository.ReadViaUserName(It.IsAny<string>()))
                .Throws(error);
            
            AuthSigninCommand command = new AuthSigninCommand(new AuthSigninModel());
            AuthSigninHandler handler = new AuthSigninHandler(this._authSearviceMock.Object, this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }
        [Fact]
        public async void AuthSigninCommand_ModelNull_Test()
        {
            AuthSigninModel model = null;
            Assert.Throws<ArgumentNullException>(() => new AuthSigninCommand(model));
        }
    }
}
