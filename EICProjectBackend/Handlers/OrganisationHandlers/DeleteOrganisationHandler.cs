using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructur.Repositories;
using LoggingServices.LogItems.Activities;
using MediatR;
using UnitOfWork;

namespace EICProjectBackend.Handlers.OrganisationHandlers
{
    public class DeleteOrganisationHandler : IRequestHandler<DeleteOrganisationCommand, Object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IRelationsRepository _relationsRepository;

        public DeleteOrganisationHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            this._unitOfWork = unitOfWork;
            this._organisationRepository = this._unitOfWork.OrganisationRepository;
            this._relationsRepository = this._unitOfWork.RelationsRepository;
        }

        public async Task<object> Handle(DeleteOrganisationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var organisation = await this._organisationRepository.Read(request.OrganisationId);
                if (organisation.IsMainOrganisation)
                {
                    var resetReset = await this._organisationRepository.ResetMainOrganisationId(request.OrganisationId);
                    if (resetReset == false)
                    {
                        this._unitOfWork.Rollback();
                        return null;
                    }

                    var projectRealtionsResult =
                        this._relationsRepository.DeleteAllOrganisationsProjects(request.OrganisationId);
                    if (projectRealtionsResult == false)
                    {
                        this._unitOfWork.Rollback();
                        return null;
                    }

                    var researcherRelationsResult =
                        this._relationsRepository.DeleteAllOrganisationResearchers(request.OrganisationId);
                    if (researcherRelationsResult == false)
                    {
                        this._unitOfWork.Rollback();
                        return null;
                    }

                    var deleteResult = this._organisationRepository.Delete(request.OrganisationId);
                    if (deleteResult == false)
                    {
                        this._unitOfWork.Rollback();
                        return null;
                    }

                    this._unitOfWork.Commit();
                    this._unitOfWork.Dispose();
                    return true;
                }
                else
                {
                    var updateProjectRelations =
                        this._relationsRepository.UpdateAllOrganisationsProjectsOrganisationId(organisation.Id,
                            (int) organisation.MainOrganisationId);
                    if (updateProjectRelations == false)
                    {
                        this._unitOfWork.Rollback();
                        return null;
                    }

                    var updateResearcherRelations =
                        this._relationsRepository.UpdateAllAllOrganisationResearchersOrganisationId(organisation.Id,
                            (int) organisation.MainOrganisationId);
                    if (updateResearcherRelations == false)
                    {
                        this._unitOfWork.Rollback();
                        return null;
                    }

                    var result = this._organisationRepository.Delete(request.OrganisationId);
                    if (result == false)
                    {
                        this._unitOfWork.Rollback();
                        return null;
                    }

                    this._unitOfWork.Commit();
                    this._unitOfWork.Dispose();
                    this._unitOfWork.QueueUserActivityLogItem(ActivityTypes.Activities.OrganisationDeleted, request.OrganisationId);
                    return true;
                }

            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.Error, 
                    ActivityTypes.Areas.DeleteOrganistionHandler, 
                    e.Message);
                return null;
            }
        }
    }

    public class DeleteOrganisationCommand : IRequest<Object>
    {
        public DeleteOrganisationCommand(int id)
        {
            if(id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            this.OrganisationId = id;
        }

        public int OrganisationId { get;}
    }
}
