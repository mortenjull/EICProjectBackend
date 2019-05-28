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
    public class ReadAllActivitiesHandler : IRequestHandler<ReadAllActivitiesCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityRepository<Activity> _ActivityRepository;

        public ReadAllActivitiesHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._ActivityRepository = this._unitOfWork.ActivityRepository;
        }

        public async Task<object> Handle(ReadAllActivitiesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = this._ActivityRepository.ReadAll().Result;
                if (result == null)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ActivityReadAll,
                    ActivityTypes.Areas.ReadAllActivityHandler,
                    e.Message);
                return false;
            }
        }
    }

    public class ReadAllActivitiesCommand : IRequest<object>
    {
        public ReadAllActivitiesCommand() { }
    }
}
