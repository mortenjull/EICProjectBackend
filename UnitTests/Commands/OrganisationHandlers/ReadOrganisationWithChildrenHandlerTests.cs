using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.ModelEntities;
using EICProjectBackend.Handlers.OrganisationHandlers;
using Moq;
using UnitOfWork;
using Xunit;

namespace UnitTests.Commands.OrganisationHandlers
{
    public class ReadOrganisationWithChildrenHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfUnitMock;
        public ReadOrganisationWithChildrenHandlerTests()
        {
            this._unitOfUnitMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async void Handle_ReadNull_Test()
        {
            OrganisationWithChildren resultnull = null;
            this._unitOfUnitMock
                .Setup(mock => mock.OrganisationRepository.ReadOrganisationWithChildren(It.IsAny<int>()))
                .Returns(Task.FromResult(resultnull));

            var command = new ReadOrganisationWithChildrenCommand(1);
            var handler = new ReadOrganisationWithChildrenHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }

        [Fact]
        public async void Handle_Succes_Test()
        {
            OrganisationWithChildren resultpre = new OrganisationWithChildren();
            this._unitOfUnitMock
                .Setup(mock => mock.OrganisationRepository.ReadOrganisationWithChildren(It.IsAny<int>()))
                .Returns(Task.FromResult(resultpre));

            var command = new ReadOrganisationWithChildrenCommand(1);
            var handler = new ReadOrganisationWithChildrenHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Equal(result, resultpre);
        }

        [Fact]
        public async void Handle_Exception_Test()
        {
            this._unitOfUnitMock
                .Setup(mock => mock.OrganisationRepository.ReadOrganisationWithChildren(It.IsAny<int>()))
                .Throws(new Exception());

            var command = new ReadOrganisationWithChildrenCommand(1);
            var handler = new ReadOrganisationWithChildrenHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void ReadOrganisationWithChildren_IdLow_Test()
        {

            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOrganisationWithChildrenCommand(0));
        }

    }
}
