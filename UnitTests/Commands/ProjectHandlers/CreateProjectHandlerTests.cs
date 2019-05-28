using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Domain.DBEntities;
using Domain.DBEntities.RelationEntities;
using Domain.ModelEntities;
using EICProjectBackend.Handlers.ProjectHandlers;
using Moq;
using UnitOfWork;
using Xunit;

namespace UnitTests.Commands.ProjectHandlers
{
    public class CreateProjectHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfUnitMock;

        public CreateProjectHandlerTests()
        {
            this._unitOfUnitMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async void handle_ProjectResutlNull_test()
        {
            Project projectnull = null;

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Create(It.IsAny<Project>())).Returns(projectnull);

            CreateProjectCommand command = new CreateProjectCommand(new CreateProjectModel(){ProjectName = "test"});
            CreateProjectHandler handler = new CreateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void handle_ProjectCategorieslNull_test()
        {
            Project project = new Project();
            ProjectCategory pc = null;

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Create(It.IsAny<Project>())).Returns(project);

            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectCategory(It.IsAny<ProjectCategory>())).Returns(pc);

            CreateProjectCommand command = new CreateProjectCommand(new CreateProjectModel() { ProjectName = "test", Categories = new List<Category>(){new Category(){CategoryId = 1,CategoryName = "test",Created = DateTime.Now, Id = 1}}});
            CreateProjectHandler handler = new CreateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void handle_ProjectresearcherNull_test()
        {
            Project project = new Project(){Id = 1};
            ProjectCategory pc = new ProjectCategory();
            ProjectResearcher pr = null;

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Create(It.IsAny<Project>())).Returns(project);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectCategory(It.IsAny<ProjectCategory>())).Returns(pc);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectResearcher(It.IsAny<ProjectResearcher>()))
                .Returns(pr);

            CreateProjectCommand command = new CreateProjectCommand(
                new CreateProjectModel()
                {
                    ProjectName = "test",
                    Categories = new List<Category>()
                    {
                        new Category() { CategoryId = 1, CategoryName = "test", Created = DateTime.Now, Id = 1 }
                    },
                    ResearcherCategories = new List<ResearcherCategory>()
                    {
                        new ResearcherCategory(){Researcher = new Researcher(){Id = 1},Category = new Category(){Id = 1}}
                    }
                });
            CreateProjectHandler handler = new CreateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void handle_OrganisationProjectNull_test()
        {
            Project project = new Project() { Id = 1 };
            ProjectCategory pc = new ProjectCategory();
            ProjectResearcher pr = new ProjectResearcher();
            OrganisationProject op = null;

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Create(It.IsAny<Project>())).Returns(project);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectCategory(It.IsAny<ProjectCategory>())).Returns(pc);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectResearcher(It.IsAny<ProjectResearcher>()))
                .Returns(pr);
            this._unitOfUnitMock.Setup(mock =>
                mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>())).Returns(op);

            CreateProjectCommand command = new CreateProjectCommand(
                new CreateProjectModel()
                {
                    ProjectName = "test",
                    Categories = new List<Category>()
                    {
                        new Category() { CategoryId = 1, CategoryName = "test", Created = DateTime.Now, Id = 1 }
                    },
                    ResearcherCategories = new List<ResearcherCategory>()
                    {
                        new ResearcherCategory(){Researcher = new Researcher(){Id = 1},Category = new Category(){Id = 1}}
                    },
                    Organisations = new List<Organisation>() { new Organisation() { Id = 1,} }
                });
            CreateProjectHandler handler = new CreateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void handle_Succes_test()
        {
            Project project = new Project() { Id = 1 };
            ProjectCategory pc = new ProjectCategory();
            ProjectResearcher pr = new ProjectResearcher();
            OrganisationProject op = new OrganisationProject();

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Create(It.IsAny<Project>())).Returns(project);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectCategory(It.IsAny<ProjectCategory>())).Returns(pc);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectResearcher(It.IsAny<ProjectResearcher>()))
                .Returns(pr);
            this._unitOfUnitMock.Setup(mock =>
                mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>())).Returns(op);

            CreateProjectCommand command = new CreateProjectCommand(
                new CreateProjectModel()
                {
                    ProjectName = "test",
                    Categories = new List<Category>()
                    {
                        new Category() { CategoryId = 1, CategoryName = "test", Created = DateTime.Now, Id = 1 }
                    },
                    ResearcherCategories = new List<ResearcherCategory>()
                    {
                        new ResearcherCategory(){Researcher = new Researcher(){Id = 1},Category = new Category(){Id = 1}}
                    },
                    Organisations = new List<Organisation>() { new Organisation() { Id = 1, } }
                });
            CreateProjectHandler handler = new CreateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Equal(result, project);
        }

        [Fact]
        public async void handle_Exception_test()
        {
            Project project = new Project() { Id = 1 };
            ProjectCategory pc = new ProjectCategory();
            ProjectResearcher pr = new ProjectResearcher();
            OrganisationProject op = new OrganisationProject();

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Create(It.IsAny<Project>())).Returns(project);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectCategory(It.IsAny<ProjectCategory>())).Returns(pc);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectResearcher(It.IsAny<ProjectResearcher>()))
                .Returns(pr);
            this._unitOfUnitMock.Setup(mock =>
                mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>())).Throws(new Exception());

            CreateProjectCommand command = new CreateProjectCommand(
                new CreateProjectModel()
                {
                    ProjectName = "test",
                    Categories = new List<Category>()
                    {
                        new Category() { CategoryId = 1, CategoryName = "test", Created = DateTime.Now, Id = 1 }
                    },
                    ResearcherCategories = new List<ResearcherCategory>()
                    {
                        new ResearcherCategory(){Researcher = new Researcher(){Id = 1},Category = new Category(){Id = 1}}
                    },
                    Organisations = new List<Organisation>() { new Organisation() { Id = 1, } }
                });
            CreateProjectHandler handler = new CreateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }

        [Fact]
        public async void CreateProjectCommand_ProjectnamenNull_test()
        {
            Assert.Throws<ArgumentNullException>(() => new CreateProjectCommand(new CreateProjectModel()));
        }
    }
}
