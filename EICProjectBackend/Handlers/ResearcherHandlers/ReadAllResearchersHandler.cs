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

    public class ReadAllResearchersHandler : IRequestHandler<ReadAllResearchersCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResearcherRepository<Researcher> _researcherRepository;

        public ReadAllResearchersHandler(IUnitOfWork unitOfWork) {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._researcherRepository = this._unitOfWork.ResearcherRepository;
        }

        public async Task<object> Handle(ReadAllResearchersCommand request, CancellationToken cancellationToken)
        { 
            try
            {
                var result = this._researcherRepository.ReadAll().Result;
                if (result == null)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                return result;
            }
            catch (SqlException e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ResearcherReadAll,
                    ActivityTypes.Areas.ReadAllResearchers,
                    e.Message);
                return false;
            }
        }
    }

    public class ReadAllResearchersCommand : IRequest<object>
    {
        public ReadAllResearchersCommand() { }
    }
}
