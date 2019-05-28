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
using UnitOfWork;

namespace EICProjectBackend.Handlers.OrganisationHandlers
{
    public class UpdateOrganisationHandler : IRequestHandler<UpdateOrganisationCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IRelationsRepository _relationsRepository;

        public UpdateOrganisationHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            this._unitOfWork = unitOfWork;
            this._organisationRepository = this._unitOfWork.OrganisationRepository;
            this._relationsRepository = this._unitOfWork.RelationsRepository;
        }

        public async Task<object> Handle(UpdateOrganisationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var organisation = new Organisation();
                organisation.Id = request.Model.Id;
                organisation.Created = DateTime.Now;
                organisation.OrganisationName = request.Model.OrganisationName;
                organisation.Address = request.Model.Address;
                if (request.Model.MainOrganisationId != -1)
                    organisation.MainOrganisationId = request.Model.MainOrganisationId;
                organisation.IsMainOrganisation = request.Model.IsMainOrganisation;
                organisation.EICColaboration = request.Model.EICColaboration;
                organisation.ZipCode = request.Model.ZipCode;
                organisation.City = request.Model.City;
                organisation.Country = request.Model.Country;

                var result = this._organisationRepository.Update(organisation);
                if (result == null)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }

                var result1 = this._relationsRepository.DeleteAllOrganisationResearchers(organisation.Id);
                if (result1 == false)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }

                if (request.Model.Researchers != null && request.Model.Researchers.Any())
                {
                    foreach (var item in request.Model.Researchers)
                    {
                        var relation = new OrganisationResearcher();
                        relation.Created = DateTime.Now;
                        relation.OrganisationId = result.Id;
                        relation.ResearcherId = item.ResearcherId;

                        var result2 = this._relationsRepository.CreateOrganisationResearcher(relation);
                        if (result2 == null)
                        {
                            this._unitOfWork.Rollback();
                            return null;
                        }
                    }
                }

                var result3 = this._relationsRepository.DeleteAllOrganisationsProjects(organisation.Id);
                if (result3 == false)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }

                if (request.Model.Projects != null && request.Model.Projects.Any())
                {
                    foreach (var item in request.Model.Projects)
                    {
                        var relation = new OrganisationProject();
                        relation.Created = DateTime.Now;
                        relation.OrganisationId = result.Id;
                        relation.ProjectId = item.ProjectId;

                        var result4 = this._relationsRepository.CreateOrganisationProject(relation);
                        if (result4 == null)
                        {
                            this._unitOfWork.Rollback();
                            return null;
                        }
                    }
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                this._unitOfWork.QueueUserActivityLogItem(ActivityTypes.Activities.OrganisationUpdated, result.Id);
                return true;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.OrganisationUpdated,
                    ActivityTypes.Areas.UpdateOrganisationHandler,
                    e.Message);
                return null;
            }
        }
    }

    public class UpdateOrganisationCommand : IRequest<object>
    {
        public UpdateOrganisationCommand(OrganisationWithChildren model)
        {
            if (model.Created == null)
                throw new ArgumentNullException(nameof(model.Created));
            if (String.IsNullOrWhiteSpace(model.OrganisationName))
                throw new ArgumentNullException(nameof(model.OrganisationName));
            if (String.IsNullOrWhiteSpace(model.Address))
                throw new ArgumentNullException(nameof(model.Address));
            if (!model.IsMainOrganisation && model.MainOrganisationId <= 0)
                throw new ArgumentNullException(nameof(model.MainOrganisationId));
            if (model.IsMainOrganisation && model.MainOrganisationId > 0)
                throw new ArgumentOutOfRangeException(nameof(model.MainOrganisationId));
            if (String.IsNullOrWhiteSpace(model.ZipCode))
                throw new ArgumentNullException(nameof(model.ZipCode));
            if (String.IsNullOrWhiteSpace(model.City))
                throw new ArgumentNullException(nameof(model.City));
            if (String.IsNullOrWhiteSpace(model.Country))
                throw new ArgumentNullException(nameof(model.Country));

            this.Model = model;
        }

        public OrganisationWithChildren Model { get; private set; }
    }
}
