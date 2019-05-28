using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities;
using EICProjectBackend.Handlers.OrganisationHandlers;
using Moq;
using UnitOfWork;
using Xunit;

namespace UnitTests.Commands.OrganisationHandlers
{
    public class ReadAllOrganisationsHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfUnitMock;

        public ReadAllOrganisationsHandlerTests()
        {
            this._unitOfUnitMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async void Handle_ResultNull_Test()
        {
            List<Organisation> resultnull = null;

            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.ReadAll()).Returns(Task.FromResult(resultnull));

            var command = new ReadAllOrganisationsCommand();
            var handler = new ReadAllOrganisationsHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }

        [Fact]
        public async void Handle_Succes_Test()
        {
            List<Organisation> resultpre = new List<Organisation>();

            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.ReadAll()).Returns(Task.FromResult(resultpre));

            var command = new ReadAllOrganisationsCommand();
            var handler = new ReadAllOrganisationsHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Equal(resultpre, result);
        }

        [Fact]
        public async void Handle_Exception_Test()
        {           
            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.ReadAll()).Throws(new Exception());

            var command = new ReadAllOrganisationsCommand();
            var handler = new ReadAllOrganisationsHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }
    }
}
