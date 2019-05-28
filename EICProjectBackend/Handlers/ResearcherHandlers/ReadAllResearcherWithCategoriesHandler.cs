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
    public class ReadAllResearcherWithCategoriesHandler : IRequestHandler<ReadAllResearcherWithCategoriesCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResearcherRepository<Researcher> _researcherRepository;

        public ReadAllResearcherWithCategoriesHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._researcherRepository = this._unitOfWork.ResearcherRepository;
        }

        public async Task<object> Handle(ReadAllResearcherWithCategoriesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = this._researcherRepository.ReadAllResearcherWithCategories().Result;
                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ResearcherReadAllWithCategories,
                    ActivityTypes.Areas.ReadAllResearchersWithCategoriesHandler,
                    e.Message);
                return false;
            }
        }
    }

    public class ReadAllResearcherWithCategoriesCommand : IRequest<object>
    {
        public ReadAllResearcherWithCategoriesCommand() { }
    }
}
