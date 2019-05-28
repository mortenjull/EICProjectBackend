using Domain.DBEntities;
using Domain.DBEntities.Entities;
using Infrastructur.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoggingServices.LogItems.Activities;
using UnitOfWork;

namespace EICProjectBackend.Handlers.ActivityHandlers
{
    public class DeleteActivityHandler : IRequestHandler<DeleteActivityCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityRepository<Activity> _ActivityRepository;

        public DeleteActivityHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._ActivityRepository = this._unitOfWork.ActivityRepository;
        }

        public async Task<object> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = this._ActivityRepository.Delete(request.Id);
                if (result == false)
                {
                    this._unitOfWork.Rollback();
                    return result;
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                this._unitOfWork.QueueUserActivityLogItem(ActivityTypes.Activities.ActivityDeleted, request.Id);
                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ActivityDeleted,
                    ActivityTypes.Areas.DeleteActivityHandler,
                    e.Message);
                return null;
            }
        }
    }

    public class DeleteActivityCommand : IRequest<object>
    {
        public int Id { get; set; }
        public DeleteActivityCommand(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            this.Id = id;
        }
    }
}
