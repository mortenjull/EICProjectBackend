using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities;
using EICProjectBackend.Handlers.ProjectHandlers;
using Moq;
using UnitOfWork;
using Xunit;

namespace UnitTests.Commands.ProjectHandlers
{
    public class ReadAllProjectsHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfUnitMock;

        public ReadAllProjectsHandlerTests()
        {
            this._unitOfUnitMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async void Handle_ResultNull_test()
        {
            List<Project> returnn = null;

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.ReadAll()).Returns(Task.FromResult(returnn));

            ReadAllProjectsCommand command = new ReadAllProjectsCommand();
            ReadAllProjectsHandler handler = new ReadAllProjectsHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }

        [Fact]
        public async void Handle_Succes_test()
        {
            List<Project> returnn = new List<Project>();

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.ReadAll()).Returns(Task.FromResult(returnn));

            ReadAllProjectsCommand command = new ReadAllProjectsCommand();
            ReadAllProjectsHandler handler = new ReadAllProjectsHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Equal(result, returnn);
        }

        [Fact]
        public async void Handle_Exception_test()
        {
            List<Project> returnn = new List<Project>();

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.ReadAll()).Throws(new Exception());

            ReadAllProjectsCommand command = new ReadAllProjectsCommand();
            ReadAllProjectsHandler handler = new ReadAllProjectsHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }
    }
}
