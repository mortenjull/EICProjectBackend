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


namespace EICProjectBackend.Handlers.ActivityHandlers
{
    public class CreateActivityHandler : IRequestHandler<CreateActivityCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityRepository<Activity> _activityRepository;


        public CreateActivityHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._activityRepository = this._unitOfWork.ActivityRepository;
        }

        public async Task<object> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Activity ac = new Activity();
                ac.Created = DateTime.Now;
                ac.ActivityName = request.Model.ActivityName;
                ac.Id = request.Model.ActivityId;

                var result = this._activityRepository.Create(ac);
                if (result == null)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                this._unitOfWork.QueueUserActivityLogItem(ActivityTypes.Activities.ActivityCreated, ac.Id);
                return result;
            }
            catch (SqlException e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ActivityCreated,
                    ActivityTypes.Areas.CreateActivityHandler,
                    e.Message);
                return false;
            }
        }
    }

    public class CreateActivityCommand : IRequest<object>
    {
        public CreateActivityCommand(ActivityModel model)
        {
            this.Model = model;
        }

        public ActivityModel Model { get; private set; }
    }
}
