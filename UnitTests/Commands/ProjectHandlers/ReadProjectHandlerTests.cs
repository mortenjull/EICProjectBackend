using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.ModelEntities;
using EICProjectBackend.Handlers.ProjectHandlers;
using Moq;
using UnitOfWork;
using Xunit;

namespace UnitTests.Commands.ProjectHandlers
{
    public class ReadProjectHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfUnitMock;

        public ReadProjectHandlerTests()
        {
            this._unitOfUnitMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async void Handle_ResultNull_Test()
        {
            ProjectReadModel returnn = null;

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.ReadProjectWithChildren(It.IsAny<int>()))
                .Returns(Task.FromResult(returnn));

            ReadProjectCommand command = new ReadProjectCommand(1);
            ReadProjectHandler handler = new ReadProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }

        [Fact]
        public async void Handle_Succes_Test()
        {
            ProjectReadModel returnn = new ProjectReadModel();

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.ReadProjectWithChildren(It.IsAny<int>()))
                .Returns(Task.FromResult(returnn));

            ReadProjectCommand command = new ReadProjectCommand(1);
            ReadProjectHandler handler = new ReadProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Equal(result, returnn);
        }

        [Fact]
        public async void Handle_Exception_Test()
        {
            ProjectReadModel returnn = new ProjectReadModel();

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.ReadProjectWithChildren(It.IsAny<int>()))
                .Throws(new Exception());

            ReadProjectCommand command = new ReadProjectCommand(1);
            ReadProjectHandler handler = new ReadProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void Command_IdOutOfRange_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadProjectCommand(0));
        }
    }
}
