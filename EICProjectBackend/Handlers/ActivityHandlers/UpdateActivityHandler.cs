using Domain.DBEntities;
using Domain.DBEntities.Entities;
using Domain.ModelEntities;
using Infrastructur.Repositories;
using MediatR;
using System; 
using System.Threading;
using System.Threading.Tasks;
using LoggingServices.LogItems.Activities;
using UnitOfWork;

namespace EICProjectBackend.Handlers.ActivityHandlers
{
    public class UpdateActivityHandler : IRequestHandler<UpdateActivityCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityRepository<Activity> _ActivityRepository;

        public UpdateActivityHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._ActivityRepository = this._unitOfWork.ActivityRepository;
        }

        public async Task<object> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Activity activity = new Activity(); 
                activity.ActivityName = request.activity.ActivityName;
                activity.Id = request.activity.ActivityId;


                var result = this._ActivityRepository.Update(activity);
                if (result == null)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                this._unitOfWork.QueueUserActivityLogItem(ActivityTypes.Activities.ActivityUpdated, activity.Id);
                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ActivityUpdated,
                    ActivityTypes.Areas.UpdateActivityHandler,
                    e.Message);
                return null;
            }
        }
    }

    public class UpdateActivityCommand : IRequest<object>
    {
        public ActivityModel activity { get; set; }
        public UpdateActivityCommand(ActivityModel activity)
        {
            this.activity = activity;
        }
    }
}
