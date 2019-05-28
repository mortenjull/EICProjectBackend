using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities;
using Domain.DBEntities.RelationEntities;
using Domain.ModelEntities;
using Infrastructur.Repositories;
using LoggingServices.LogItems.Activities;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnitOfWork;

namespace EICProjectBackend.Handlers.ProjectHandlers
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, Object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Project> _projectRepository;
        private readonly IRelationsRepository _relationsRepository;

        public CreateProjectHandler(IUnitOfWork unitOfWork)
        {
            if(unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
            this._unitOfWork = unitOfWork;
            this._projectRepository = this._unitOfWork.ProjectRepository;
            this._relationsRepository = this._unitOfWork.RelationsRepository;
        }

        public async Task<object> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project project = new Project();
                
                project.Created = DateTime.Now;
                project.ProjectName = request.Model.ProjectName;
                project.StartDate = request.Model.StartDate; // Check if date is valid
                project.EndDate = request.Model.EndDate;

                var projectResult = this._projectRepository.Create(project);
                if (projectResult == null)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }

                foreach (var cat in request.Model.Categories)
                {
                    ProjectCategory projectCategory = new ProjectCategory();
                    projectCategory.Created = DateTime.Now;
                    projectCategory.CategoryId = cat.Id;
                    projectCategory.ProjectId = projectResult.Id;
               
                    var catResult = this._relationsRepository.CreateProjectCategory(projectCategory);
                    if (catResult == null)
                    {
                        this._unitOfWork.Rollback();
                        return false;
                    }
                }

                foreach (var researcherCategory in request.Model.ResearcherCategories)
                {
                    ProjectResearcher projectResearcher = new ProjectResearcher();
                    projectResearcher.Created = DateTime.Now;
                    projectResearcher.ResearcherId = researcherCategory.Researcher.Id;
                    projectResearcher.ProjectId = projectResult.Id;
                    projectResearcher.CategoryId = researcherCategory.Category.Id;
                    projectResearcher.TitleId = 1;

                    var researcherResult = this._relationsRepository.CreateProjectResearcher(projectResearcher);
                    if (researcherResult == null)
                    {
                        this._unitOfWork.Rollback();
                        return false;
                    }
                }

                foreach (var organisation in request.Model.Organisations)
                {
                    OrganisationProject organisationProject = new OrganisationProject();
                    organisationProject.Created = DateTime.Now;
                    organisationProject.OrganisationId = organisation.Id;
                    organisationProject.ProjectId = projectResult.Id; 

                    var orgaResult = this._relationsRepository.CreateOrganisationProject(organisationProject);
                    if (orgaResult == null)
                    {
                        this._unitOfWork.Rollback();
                        return false;
                    }

                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                this._unitOfWork.QueueUserActivityLogItem(ActivityTypes.Activities.ProjectCreated, projectResult.Id);
                return projectResult;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ProjectCreated,
                    ActivityTypes.Areas.CreateProjectHandler,
                    e.Message);
                return null;
            }
        }
    }

    public class CreateProjectCommand : IRequest<Object>
    {
        public CreateProjectCommand(CreateProjectModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ProjectName))
                throw new ArgumentNullException(nameof(model.ProjectName));

            this.Model = model;
        }

        public CreateProjectModel Model { get; set; }
    }
}
