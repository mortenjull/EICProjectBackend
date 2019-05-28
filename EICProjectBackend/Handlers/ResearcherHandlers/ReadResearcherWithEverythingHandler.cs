using Domain.DBEntities;
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


namespace EICProjectBackend.Handlers.ResearcherHandlers
{
    public class ReadResearcherWithEverythingHandler : IRequestHandler<ReadResearcherWithEverythingCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResearcherRepository<Researcher> _researcherRepository;

        public ReadResearcherWithEverythingHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._researcherRepository = this._unitOfWork.ResearcherRepository;
        }

        public async Task<object> Handle(ReadResearcherWithEverythingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = this._researcherRepository.ReadResearcherWithEverything(request.Id).Result;
                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ResearcherReadWithEverything,
                    ActivityTypes.Areas.ReadResearcherWithEverythingHandler,
                    e.Message);
                return false;
            }
        }
    }
    public class ReadResearcherWithEverythingCommand : IRequest<object>
    {
        public int Id { get; set; }
        public ReadResearcherWithEverythingCommand(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            this.Id = id;
        }
    }
}
