using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Domain.DBEntities;
using Domain.DBEntities.RelationEntities;
using Domain.ModelEntities;
using EICProjectBackend.Handlers.OrganisationHandlers;
using Moq;
using UnitOfWork;
using Xunit;

namespace UnitTests.Commands.OrganisationHandlers
{
    public class UpdateOrganisationHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfUnitMock;
        public UpdateOrganisationHandlerTests()
        {
            this._unitOfUnitMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async void Handle_ACouldNotUpdateOrganisation_Test()
        {
            Organisation organisationNull = null;
            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.Update(It.IsAny<Organisation>()))
                .Returns(organisationNull);

            UpdateOrganisationCommand command = new UpdateOrganisationCommand(new OrganisationWithChildren()
            {
                Created = DateTime.Now,
                OrganisationName = "s",
                Address = "s",
                MainOrganisationId = 1,
                ZipCode = "6700",
                City = "es",
                Country = "sad"
            });

            var handler = new UpdateOrganisationHandler(this._unitOfUnitMock.Object);
            var result = await handler.Handle(command, new CancellationTokenSource().Token);
            Assert.Null(result);
        }

        [Fact]
        public async void Handle_BCouldNotDeleteOrganisationResearchers_Test()
        {
            Organisation organisationNull = new Organisation();
            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.Update(It.IsAny<Organisation>()))
                .Returns(organisationNull);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.DeleteAllOrganisationResearchers(It.IsAny<int>()))
                .Returns(false);

            UpdateOrganisationCommand command = new UpdateOrganisationCommand(new OrganisationWithChildren()
            {
                Created = DateTime.Now,
                OrganisationName = "s",
                Address = "s",
                MainOrganisationId = 1,
                ZipCode = "6700",
                City = "es",
                Country = "sad"
            });            
            var handler = new UpdateOrganisationHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);
            Assert.Null(result);
        }

        [Fact]
        public async void Handle_CCouldNotCreateOrganisationResearchers_Test()
        {
            Organisation organisationNull = new Organisation();
            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.Update(It.IsAny<Organisation>()))
                .Returns(organisationNull);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.DeleteAllOrganisationResearchers(It.IsAny<int>()))
                .Returns(true);

            OrganisationResearcher nullItem = null;
            this._unitOfUnitMock.Setup(mock =>
                    mock.RelationsRepository.CreateOrganisationResearcher(It.IsAny<OrganisationResearcher>()))
                .Returns(nullItem);

            UpdateOrganisationCommand command = new UpdateOrganisationCommand(new OrganisationWithChildren()
            {
                Created = DateTime.Now,
                OrganisationName = "s",
                Address = "s",
                MainOrganisationId = 1,
                ZipCode = "6700",
                City = "es",
                Country = "sad",
                Researchers = new List<ResearcherModel>() { new ResearcherModel() }
            });
            var handler = new UpdateOrganisationHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);
            Assert.Null(result);
        }

        [Fact]
        public async void Handle_DCouldNotDeleteOrganisationProjects_Test()
        {
            Organisation organisationNull = new Organisation();
            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.Update(It.IsAny<Organisation>()))
                .Returns(organisationNull);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.DeleteAllOrganisationResearchers(It.IsAny<int>()))
                .Returns(true);

            OrganisationResearcher nullItem = new OrganisationResearcher();
            this._unitOfUnitMock.Setup(mock =>
                    mock.RelationsRepository.CreateOrganisationResearcher(It.IsAny<OrganisationResearcher>()))
                .Returns(nullItem);

            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteAllOrganisationsProjects(It.IsAny<int>()))
                .Returns(false);

            UpdateOrganisationCommand command = new UpdateOrganisationCommand(new OrganisationWithChildren()
            {
                Created = DateTime.Now,
                OrganisationName = "s",
                Address = "s",
                MainOrganisationId = 1,
                ZipCode = "6700",
                City = "es",
                Country = "sad",
                Researchers = new List<ResearcherModel>() { new ResearcherModel() }
            });
            var handler = new UpdateOrganisationHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);
            Assert.Null(result);
        }

        [Fact]
        public async void Handle_ECouldNotCreateOrganisationProjects_Test()
        {
            Organisation organisationNull = new Organisation();
            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.Update(It.IsAny<Organisation>()))
                .Returns(organisationNull);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.DeleteAllOrganisationResearchers(It.IsAny<int>()))
                .Returns(true);

            OrganisationResearcher nullItem = new OrganisationResearcher();
            this._unitOfUnitMock.Setup(mock =>
                    mock.RelationsRepository.CreateOrganisationResearcher(It.IsAny<OrganisationResearcher>()))
                .Returns(nullItem);

            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteAllOrganisationsProjects(It.IsAny<int>()))
                .Returns(true);

            OrganisationProject nullProejct = null;
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>()))
                .Returns(nullProejct);

            UpdateOrganisationCommand command = new UpdateOrganisationCommand(new OrganisationWithChildren()
            {
                Created = DateTime.Now,
                OrganisationName = "s",
                Address = "s",
                MainOrganisationId = 1,
                ZipCode = "6700",
                City = "es",
                Country = "sad",
                Researchers = new List<ResearcherModel>() { new ResearcherModel() },
                Projects = new List<ProjectModel>() { new ProjectModel() }
            });
            var handler = new UpdateOrganisationHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);
            Assert.Null(result);
        }

        [Fact]
        public async void Handle_FSucces_Test()
        {
            Organisation organisationNull = new Organisation();
            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.Update(It.IsAny<Organisation>()))
                .Returns(organisationNull);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.DeleteAllOrganisationResearchers(It.IsAny<int>()))
                .Returns(true);

            OrganisationResearcher nullItem = new OrganisationResearcher();
            this._unitOfUnitMock.Setup(mock =>
                    mock.RelationsRepository.CreateOrganisationResearcher(It.IsAny<OrganisationResearcher>()))
                .Returns(nullItem);

            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteAllOrganisationsProjects(It.IsAny<int>()))
                .Returns(true);

            OrganisationProject nullProejct = new OrganisationProject();
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>()))
                .Returns(nullProejct);

            UpdateOrganisationCommand command = new UpdateOrganisationCommand(new OrganisationWithChildren()
            {
                Created = DateTime.Now,
                OrganisationName = "s",
                Address = "s",
                MainOrganisationId = 1,
                ZipCode = "6700",
                City = "es",
                Country = "sad",
                Researchers = new List<ResearcherModel>() { new ResearcherModel() },
                Projects = new List<ProjectModel>() { new ProjectModel() }
            });

            var handler = new UpdateOrganisationHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);
            Assert.True((bool)result);
        }




        [Fact]
        public async void UpdateOrganisationCommand_CreatedNull_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();

            Assert.Throws<ArgumentNullException>(() => new UpdateOrganisationCommand(model));
        }

        [Fact]
        public async void UpdateOrganisationCommand_OrganisationNameEmpty_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "";

            Assert.Throws<ArgumentNullException>(() => new UpdateOrganisationCommand(model));
        }

        [Fact]
        public async void UpdateOrganisationCommand_AddressEmpty_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "asd";
            model.Address = "";

            Assert.Throws<ArgumentNullException>(() => new UpdateOrganisationCommand(model));
        }

        [Fact]
        public async void UpdateOrganisationCommand_MainOrganisationIdLow_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "asd";
            model.Address = "asd";
            model.IsMainOrganisation = false;
            model.MainOrganisationId = -1;

            Assert.Throws<ArgumentNullException>(() => new UpdateOrganisationCommand(model));
        }

        [Fact]
        public async void UpdateOrganisationCommand_MainOrganisationIdHigh_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "asd";
            model.Address = "asd";
            model.IsMainOrganisation = true;
            model.MainOrganisationId = 1;

            Assert.Throws<ArgumentOutOfRangeException>(() => new UpdateOrganisationCommand(model));
        }

        [Fact]
        public async void UpdateOrganisationCommand_ZipCodeNull_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "asd";
            model.Address = "asd";
            model.IsMainOrganisation = false;
            model.MainOrganisationId = 1;
            model.ZipCode = "";

            Assert.Throws<ArgumentNullException>(() => new UpdateOrganisationCommand(model));
        }

        [Fact]
        public async void UpdateOrganisationCommand_CityNull_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "asd";
            model.Address = "asd";
            model.IsMainOrganisation = false;
            model.MainOrganisationId = 1;
            model.ZipCode = "asd";
            model.City = "";

            Assert.Throws<ArgumentNullException>(() => new UpdateOrganisationCommand(model));
        }

        [Fact]
        public async void UpdateOrganisationCommand_CountryNull_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "asd";
            model.Address = "asd";
            model.IsMainOrganisation = false;
            model.MainOrganisationId = 1;
            model.ZipCode = "asd";
            model.City = "asd";
            model.Country = "";

            Assert.Throws<ArgumentNullException>(() => new UpdateOrganisationCommand(model));
        }
    }
}
