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

namespace EICProjectBackend.Handlers.ActivityHandlers.ActivityResearcherHandlers
{
    public class ReadAllActivityForResearcherHandler : IRequestHandler<ReadAllActivityForResearcherCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityRepository<Activity> _ActivityRepository;

        public ReadAllActivityForResearcherHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._ActivityRepository = this._unitOfWork.ActivityRepository;
        }

        public async Task<object> Handle(ReadAllActivityForResearcherCommand request, CancellationToken cancellationToken)
        { 
            try
            {
                var result = await this._ActivityRepository.ReadAllActivityForResearcher(request.Id);
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
                    ActivityTypes.Activities.ActivityOfResearcherReadALL,
                    ActivityTypes.Areas.ReadAllActivityResearchHandler,
                    e.Message);
                return false;
            }
            
        }
    }

    public class ReadAllActivityForResearcherCommand : IRequest<object>
    {
        public int Id { get; set; }
        public ReadAllActivityForResearcherCommand(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            this.Id = id;
        }
    }
}
