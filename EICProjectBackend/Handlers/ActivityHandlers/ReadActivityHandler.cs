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
    public class ReadActivityHandler : IRequestHandler<ReadActivityCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityRepository<Activity> _ActivityRepository;

        public ReadActivityHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._ActivityRepository = this._unitOfWork.ActivityRepository;
        }


        public async Task<object> Handle(ReadActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = this._ActivityRepository.Read(request.Id).Result; 
                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ActivityRead,
                    ActivityTypes.Areas.ReadActivityHandler,
                    e.Message);
                return false;
            }
        }
    }

    public class ReadActivityCommand : IRequest<object>
    {
        public int Id { get; set; }
        public ReadActivityCommand(int id) {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id)); 
            this.Id = id; 
        }
    }
}
