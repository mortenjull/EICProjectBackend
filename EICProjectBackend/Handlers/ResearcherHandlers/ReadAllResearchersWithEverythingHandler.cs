using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities;
using Infrastructur.Repositories;
using LoggingServices.LogItems.Activities;
using MediatR;
using UnitOfWork;

namespace EICProjectBackend.Handlers.ResearcherHandlers
{
    public class ReadAllResearchersWithEverythingHandler : IRequestHandler<ReadAllResearchersWithEverythingCommand, Object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private IResearcherRepository<Researcher> _researcherRepository;

        public ReadAllResearchersWithEverythingHandler(IUnitOfWork unitOfWork)
        {
            if(unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            this._unitOfWork = unitOfWork;
            this._researcherRepository = this._unitOfWork.ResearcherRepository;
        }

        public async Task<object> Handle(ReadAllResearchersWithEverythingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._researcherRepository.ReadResearchersWithEverything();
                if (result == null)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ResearcherReadAllWithEverything,
                    ActivityTypes.Areas.ReadAllResearchersWithEverythingHandler,
                    e.Message);
                return null;
            }
        }
    }

    public class ReadAllResearchersWithEverythingCommand : IRequest<Object>
    {
        public ReadAllResearchersWithEverythingCommand() { }
    }
}
