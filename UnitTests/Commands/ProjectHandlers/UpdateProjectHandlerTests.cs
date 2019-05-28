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
    public class UpdateProjectHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfUnitMock;

        public UpdateProjectHandlerTests()
        {
            this._unitOfUnitMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async void handle_ProjectResutlNull_test()
        {
            Project returnn = null;

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Update(It.IsAny<Project>())).Returns(returnn);

            UpdateProjectCommand command = new UpdateProjectCommand(new CreateProjectModel() { ProjectName = "test" });
            UpdateProjectHandler handler = new UpdateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }

        [Fact]
        public async void handle_DeleteProjectOrganisationResultlNull_test()
        {
            Project returnn = new Project();

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Update(It.IsAny<Project>())).Returns(returnn);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteProjectOrganisation(It.IsAny<int>()))
                .Returns(false); 

            UpdateProjectCommand command = new UpdateProjectCommand(new CreateProjectModel() { ProjectName = "test" });
            UpdateProjectHandler handler = new UpdateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }

        [Fact]
        public async void handle_CreateOrganisationProjectResultlNull_test()
        {
            Project returnn = new Project();
            OrganisationProject op = null;

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Update(It.IsAny<Project>())).Returns(returnn);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteProjectOrganisation(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock.Setup(mock =>
                mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>())).Returns(op);

            UpdateProjectCommand command = new UpdateProjectCommand(new CreateProjectModel()
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
            UpdateProjectHandler handler = new UpdateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void handle_DeleteCategoriesProjectResultlNull_test()
        {
            Project returnn = new Project();
            OrganisationProject op = new OrganisationProject();

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Update(It.IsAny<Project>())).Returns(returnn);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteProjectOrganisation(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock.Setup(mock =>
                mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>())).Returns(op);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteCategoriesProject(It.IsAny<int>()))
                .Returns(false);

            UpdateProjectCommand command = new UpdateProjectCommand(new CreateProjectModel()
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
            UpdateProjectHandler handler = new UpdateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }

        [Fact]
        public async void handle_CreateProjectCategoryResultlNull_test()
        {
            Project returnn = new Project();
            OrganisationProject op = new OrganisationProject();
            ProjectCategory pc = null;

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Update(It.IsAny<Project>())).Returns(returnn);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteProjectOrganisation(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock.Setup(mock =>
                mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>())).Returns(op);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteCategoriesProject(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectCategory(It.IsAny<ProjectCategory>())).Returns(pc);

            UpdateProjectCommand command = new UpdateProjectCommand(new CreateProjectModel()
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
            UpdateProjectHandler handler = new UpdateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void handle_DeleteProjectResearcherWithProjectIdResultlNull_test()
        {
            Project returnn = new Project();
            OrganisationProject op = new OrganisationProject();
            ProjectCategory pc = new ProjectCategory();

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Update(It.IsAny<Project>())).Returns(returnn);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteProjectOrganisation(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock.Setup(mock =>
                mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>())).Returns(op);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteCategoriesProject(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectCategory(It.IsAny<ProjectCategory>())).Returns(pc);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.DeleteProjectResearcherWithProjectId(It.IsAny<int>()))
                .Returns(false);

            UpdateProjectCommand command = new UpdateProjectCommand(new CreateProjectModel()
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
            UpdateProjectHandler handler = new UpdateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }

        [Fact]
        public async void handle_CreateProjectResearcherResultlNull_test()
        {
            Project returnn = new Project();
            OrganisationProject op = new OrganisationProject();
            ProjectCategory pc = new ProjectCategory();
            ProjectResearcher pr = null;

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Update(It.IsAny<Project>())).Returns(returnn);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteProjectOrganisation(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock.Setup(mock =>
                mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>())).Returns(op);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteCategoriesProject(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectCategory(It.IsAny<ProjectCategory>())).Returns(pc);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.DeleteProjectResearcherWithProjectId(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectResearcher(It.IsAny<ProjectResearcher>()))
                .Returns(pr);

            UpdateProjectCommand command = new UpdateProjectCommand(new CreateProjectModel()
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
            UpdateProjectHandler handler = new UpdateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.False((bool)result);
        }

        [Fact]
        public async void handle_Succes_test()
        {
            Project returnn = new Project();
            OrganisationProject op = new OrganisationProject();
            ProjectCategory pc = new ProjectCategory();
            ProjectResearcher pr = new ProjectResearcher();

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Update(It.IsAny<Project>())).Returns(returnn);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteProjectOrganisation(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock.Setup(mock =>
                mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>())).Returns(op);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteCategoriesProject(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectCategory(It.IsAny<ProjectCategory>())).Returns(pc);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.DeleteProjectResearcherWithProjectId(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectResearcher(It.IsAny<ProjectResearcher>()))
                .Returns(pr);

            UpdateProjectCommand command = new UpdateProjectCommand(new CreateProjectModel()
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
            UpdateProjectHandler handler = new UpdateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.True((bool)result);
        }

        [Fact]
        public async void handle_Exception_test()
        {
            Project returnn = new Project();
            OrganisationProject op = new OrganisationProject();
            ProjectCategory pc = new ProjectCategory();
            ProjectResearcher pr = new ProjectResearcher();

            this._unitOfUnitMock.Setup(mock => mock.ProjectRepository.Update(It.IsAny<Project>())).Returns(returnn);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteProjectOrganisation(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock.Setup(mock =>
                mock.RelationsRepository.CreateOrganisationProject(It.IsAny<OrganisationProject>())).Returns(op);
            this._unitOfUnitMock.Setup(mock => mock.RelationsRepository.DeleteCategoriesProject(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectCategory(It.IsAny<ProjectCategory>())).Returns(pc);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.DeleteProjectResearcherWithProjectId(It.IsAny<int>()))
                .Returns(true);
            this._unitOfUnitMock
                .Setup(mock => mock.RelationsRepository.CreateProjectResearcher(It.IsAny<ProjectResearcher>()))
                .Throws(new Exception());

            UpdateProjectCommand command = new UpdateProjectCommand(new CreateProjectModel()
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
            UpdateProjectHandler handler = new UpdateProjectHandler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }

    }
}
