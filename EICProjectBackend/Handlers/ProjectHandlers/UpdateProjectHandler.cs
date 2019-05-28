using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities;
using Domain.DBEntities.RelationEntities;
using Domain.ModelEntities;
using Infrastructur.Repositories;
using MediatR;
using UnitOfWork;

namespace EICProjectBackend.Handlers.ProjectHandlers
{
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepository _projectRepository;
        private readonly IRelationsRepository _relationsRepository;

        public UpdateProjectHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            this._unitOfWork = unitOfWork;
            this._projectRepository = this._unitOfWork.ProjectRepository;
            this._relationsRepository = this._unitOfWork.RelationsRepository;
        }

        public async Task<object> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var project = new Project(); 
                project.Created = DateTime.Now;
                project.StartDate = request.Model.StartDate;
                project.EndDate = request.Model.EndDate;
                project.ProjectName = request.Model.ProjectName;
                project.Id = request.Model.Id;
                

                var result = this._projectRepository.Update(project);
                if (result == null)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }
                
                // OrganisationProject
                var result1 = this._relationsRepository.DeleteProjectOrganisation(result.Id);
                if (result1 == false)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }
                foreach (var organisation in request.Model.Organisations)
                {
                    OrganisationProject organisationProject = new OrganisationProject();
                    organisationProject.Created = DateTime.Now;
                    organisationProject.OrganisationId = organisation.OrganisationId;
                    organisationProject.ProjectId = result.Id;

                    var orgaResult = this._relationsRepository.CreateOrganisationProject(organisationProject);
                    if (orgaResult == null)
                    {
                        this._unitOfWork.Rollback();
                        return false;
                    }

                }
                // CategoriesProject 
                var result2 = this._relationsRepository.DeleteCategoriesProject(result.Id);
                if (result2 == false)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }
                foreach (var cat in request.Model.Categories)
                {
                    ProjectCategory projectCategory = new ProjectCategory();
                    projectCategory.Created = DateTime.Now;
                    projectCategory.CategoryId = cat.CategoryId;
                    projectCategory.ProjectId = result.Id;

                    var catResult = this._relationsRepository.CreateProjectCategory(projectCategory);
                    if (catResult == null)
                    {
                        this._unitOfWork.Rollback();
                        return false;
                    }
                }

                 var result3 = this._relationsRepository.DeleteProjectResearcherWithProjectId(result.Id);
                 if (result3 == false)
                 {
                     this._unitOfWork.Rollback();
                     return null;
                 }
                foreach (var researcherCategory in request.Model.ResearcherCategories)
                {
                    ProjectResearcher projectResearcher = new ProjectResearcher();
                    projectResearcher.Created = DateTime.Now;
                    projectResearcher.ResearcherId = researcherCategory.Researcher.Id;
                    projectResearcher.ProjectId = result.Id;
                    projectResearcher.CategoryId = researcherCategory.Category.Id;
                    projectResearcher.TitleId = 1;

                    var researcherResult = this._relationsRepository.CreateProjectResearcher(projectResearcher);
                    if (researcherResult == null)
                    {
                        this._unitOfWork.Rollback();
                        return false;
                    }
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                return true;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                return null;
            }
        }
    }

    public class UpdateProjectCommand : IRequest<object>
    {
        public UpdateProjectCommand(CreateProjectModel model)
        {
            //Need checks
            this.Model = model;
        }

        public CreateProjectModel Model { get; private set; }
    }
}
