using Domain.DBEntities;
using Infrastructur.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities.Entities;
using LoggingServices.LogItems.Activities;
using UnitOfWork;


namespace EICProjectBackend.Handlers.ResearcherHandlers
{
    public class DeleteResearcherHandler : IRequestHandler<DeleteResearcherCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResearcherRepository<Researcher> _researcherRepository;
        private readonly IRelationsRepository _relationsRepository;
        private readonly IActivityRepository<Activity> _activityRepository;

        public DeleteResearcherHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._researcherRepository = this._unitOfWork.ResearcherRepository;
            this._relationsRepository = this._unitOfWork.RelationsRepository;
            this._activityRepository = this._unitOfWork.ActivityRepository;
        }

        public async Task<object> Handle(DeleteResearcherCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var researcherOrgaResult = this._relationsRepository.DeleteOrganisationResearcher(request.Id);
                if (researcherOrgaResult == false)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }
                var projectReasercherResult = this._relationsRepository.DeleteProjectResearcher(request.Id);
                if (projectReasercherResult == false)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }
                var reseacherCateResult = this._relationsRepository.DeleteResearcherCategoryViaReseacherID(request.Id);
                if (reseacherCateResult == false)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }
                var activityResearcherResult =
                    this._activityRepository.DeleteActivityReseacherViaResearcherId(request.Id);
                if (activityResearcherResult == false)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }
                var result = this._researcherRepository.Delete(request.Id);
                if (result == false)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                this._unitOfWork.QueueUserActivityLogItem(ActivityTypes.Activities.ResearcherDeleted, request.Id);
                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ResearcherDeleted,
                    ActivityTypes.Areas.DeleteResearcherHandler,
                    e.Message);
                return false;
            }
        }
    }

    public class DeleteResearcherCommand : IRequest<object>
    {
        public int Id { get; set; }
        public DeleteResearcherCommand(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            this.Id = id;
        }
    }
}
