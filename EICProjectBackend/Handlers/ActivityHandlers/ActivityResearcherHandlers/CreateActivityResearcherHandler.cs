using Domain.DBEntities;
using Domain.DBEntities.Entities;
using Domain.ModelEntities;
using Infrastructur.Repositories;
using MediatR;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using LoggingServices.LogItems.Activities;
using UnitOfWork;


namespace EICProjectBackend.Handlers.ActivityHandlers.ActivityResearcherHandlers
{
    public class CreateActivityResearcherHandler : IRequestHandler<CreateActivityResearcherCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityRepository<Activity> _activityRepository;
        

        public CreateActivityResearcherHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._activityRepository = this._unitOfWork.ActivityRepository;
        }

        public async Task<object> Handle(CreateActivityResearcherCommand request, CancellationToken cancellationToken)
        {
            try
            {
                

                ActivityResearcher uam = new ActivityResearcher();
                uam.Created = DateTime.Now;
                uam.UserId = request.Model.UserId;
                uam.ActivityId = request.Model.ActivityId;
                uam.ResearcherId = request.Model.ResearcherId;

                var result = this._activityRepository.CreateActivityResearcher(uam);
                if (result == false)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                this._unitOfWork.QueueUserActivityLogItem(ActivityTypes.Activities.ActivityAddedToResearcher, uam.ActivityId);
                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ActivityAddedToResearcher,
                    ActivityTypes.Areas.CreateActivityResearchHandler,
                    e.Message);

                return false;
            }
        }
    }

    public class CreateActivityResearcherCommand : IRequest<object>
    {
        public CreateActivityResearcherCommand(ActivityResearcher model)
        {
            this.Model = model;
        }

        public ActivityResearcher Model { get; private set; }
    }
}
