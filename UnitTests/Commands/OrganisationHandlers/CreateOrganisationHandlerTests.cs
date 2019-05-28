using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities;
using Domain.DBEntities.RelationEntities;
using Domain.ModelEntities;
using EICProjectBackend.Handlers.OrganisationHandlers;
using UnitOfWork;
using Xunit;

namespace UnitTests.Commands.OrganisationHandlers
{    

    public class CreateOrganisationHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfUnitMock;

        public CreateOrganisationHandlerTests()
        {
            this._unitOfUnitMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async void Handle_CouldNotCreateOrganisation_Test()
        {
            Organisation organisationNull = null;
            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.Create(It.IsAny<Organisation>()))
                .Returns(organisationNull);

            CreateOrganisationCommand command = new CreateOrganisationCommand(new OrganisationWithChildren()
            {
                Created = DateTime.Now,
                OrganisationName = "s",
                Address = "s",
                MainOrganisationId = 1,
                ZipCode = "6700",
                City = "es",
                Country = "sad"
            });

            var handler = new CreateOrganisationHandler(this._unitOfUnitMock.Object);
            var result = await handler.Handle(command, new CancellationTokenSource().Token);
            Assert.Null(result);
        }

        [Fact]
        public async void Handle_CouldNotCreateOrganisationResearcher_Test()
        {
            Organisation organisation = new Organisation(){};
            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.Create(It.IsAny<Organisation>()))
                .Returns(organisation);

            OrganisationResearcher nullItem = null;
            this._unitOfUnitMock.Setup(mock =>
                    mock.RelationsRepository.CreateOrganisationResearcher(It.IsAny<OrganisationResearcher>()))
                .Returns(nullItem);

            CreateOrganisationCommand command = new CreateOrganisationCommand(new OrganisationWithChildren()
            {
                Created = DateTime.Now,
                OrganisationName = "s",
                Address = "s",
                MainOrganisationId = 1,
                ZipCode = "6700",
                City = "es",
                Country = "sad",
                Researchers = new List<ResearcherModel>() {new ResearcherModel()}
            });

            var handler = new CreateOrganisationHandler(this._unitOfUnitMock.Object);
            var result = await handler.Handle(command, new CancellationTokenSource().Token);
            Assert.Null(result);
        }

        [Fact]
        public async void Handle_CouldNotCreateOrganisationProjects_Test()
        {
            Organisation organisation = new Organisation() { };
            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.Create(It.IsAny<Organisation>()))
                .Returns(organisation);

            OrganisationResearcher nullItem = new OrganisationResearcher();
            this._unitOfUnitMock.Setup(mock =>
                    mock.RelationsRepository.CreateOrganisationResearcher(It.IsAny<OrganisationResearcher>()))
                .Returns(nullItem);

            OrganisationProject nullProejct = null;
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>()))
                .Returns(nullProejct);

            CreateOrganisationCommand command = new CreateOrganisationCommand(new OrganisationWithChildren()
            {
                Created = DateTime.Now,
                OrganisationName = "s",
                Address = "s",
                MainOrganisationId = 1,
                ZipCode = "6700",
                City = "es",
                Country = "sad",
                Researchers = new List<ResearcherModel>() { new ResearcherModel() },
                Projects = new List<ProjectModel>() { new ProjectModel()}
            });

            var handler = new CreateOrganisationHandler(this._unitOfUnitMock.Object);
            var result = await handler.Handle(command, new CancellationTokenSource().Token);
            Assert.Null(result);
        }

        [Fact]
        public async void Handle_OrganisationCreated_Test()
        {
            Organisation organisation = new Organisation() { };
            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.Create(It.IsAny<Organisation>()))
                .Returns(organisation);

            OrganisationResearcher nullItem = new OrganisationResearcher();
            this._unitOfUnitMock.Setup(mock =>
                    mock.RelationsRepository.CreateOrganisationResearcher(It.IsAny<OrganisationResearcher>()))
                .Returns(nullItem);

            OrganisationProject nullProejct = new OrganisationProject();
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>()))
                .Returns(nullProejct);

            CreateOrganisationCommand command = new CreateOrganisationCommand(new OrganisationWithChildren()
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

            var handler = new CreateOrganisationHandler(this._unitOfUnitMock.Object);
            var result = await handler.Handle(command, new CancellationTokenSource().Token);
            Assert.True((bool)result);
        }

        [Fact]
        public async void Handle_OrganisationException_Test()
        {
            Organisation organisation = new Organisation() { };
            this._unitOfUnitMock.Setup(mock => mock.OrganisationRepository.Create(It.IsAny<Organisation>()))
                .Returns(organisation);

            OrganisationResearcher nullItem = new OrganisationResearcher();
            this._unitOfUnitMock.Setup(mock =>
                    mock.RelationsRepository.CreateOrganisationResearcher(It.IsAny<OrganisationResearcher>()))
                .Returns(nullItem);

            OrganisationProject nullProejct = new OrganisationProject();
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>()))
                .Throws(new Exception());

            CreateOrganisationCommand command = new CreateOrganisationCommand(new OrganisationWithChildren()
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

            var handler = new CreateOrganisationHandler(this._unitOfUnitMock.Object);
            var result = await handler.Handle(command, new CancellationTokenSource().Token);
            Assert.Null(result);
        }

        [Fact]
        public async void CreateOrganisationCommand_CreatedNull_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            
            Assert.Throws<ArgumentNullException>(() => new CreateOrganisationCommand(model));
        }

        [Fact]
        public async void CreateOrganisationCommand_OrganisationNameEmpty_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "";

            Assert.Throws<ArgumentNullException>(() => new CreateOrganisationCommand(model));
        }

        [Fact]
        public async void CreateOrganisationCommand_AddressEmpty_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "asd";
            model.Address = "";

            Assert.Throws<ArgumentNullException>(() => new CreateOrganisationCommand(model));
        }

        [Fact]
        public async void CreateOrganisationCommand_MainOrganisationIdLow_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "asd";
            model.Address = "asd";
            model.IsMainOrganisation = false;
            model.MainOrganisationId = -1;

            Assert.Throws<ArgumentNullException>(() => new CreateOrganisationCommand(model));
        }

        [Fact]
        public async void CreateOrganisationCommand_MainOrganisationIdHigh_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "asd";
            model.Address = "asd";
            model.IsMainOrganisation = true;
            model.MainOrganisationId = 1;

            Assert.Throws<ArgumentOutOfRangeException>(() => new CreateOrganisationCommand(model));
        }

        [Fact]
        public async void CreateOrganisationCommand_ZipCodeNull_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "asd";
            model.Address = "asd";
            model.IsMainOrganisation = false;
            model.MainOrganisationId = 1;
            model.ZipCode = "";

            Assert.Throws<ArgumentNullException>(() => new CreateOrganisationCommand(model));
        }

        [Fact]
        public async void CreateOrganisationCommand_CityNull_Test()
        {
            OrganisationWithChildren model = new OrganisationWithChildren();
            model.Created = DateTime.Now;
            model.OrganisationName = "asd";
            model.Address = "asd";
            model.IsMainOrganisation = false;
            model.MainOrganisationId = 1;
            model.ZipCode = "asd";
            model.City = "";

            Assert.Throws<ArgumentNullException>(() => new CreateOrganisationCommand(model));
        }

        [Fact]
        public async void CreateOrganisationCommand_CountryNull_Test()
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

            Assert.Throws<ArgumentNullException>(() => new CreateOrganisationCommand(model));
        }
    }
}
