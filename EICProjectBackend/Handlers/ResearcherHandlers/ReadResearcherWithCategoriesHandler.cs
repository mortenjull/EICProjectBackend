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
    public class ReadResearcherWithCategoriesHandler : IRequestHandler<ReadResearcherWithCategoriesCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResearcherRepository<Researcher> _researcherRepository;

        public ReadResearcherWithCategoriesHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._researcherRepository = this._unitOfWork.ResearcherRepository;
        }

        public async Task<object> Handle(ReadResearcherWithCategoriesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = this._researcherRepository.ReadResearcherWithCategories(request.Id).Result;
                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ResearcherReadWithCategories,
                    ActivityTypes.Areas.ReadResearcherWithCategoriesHandler,
                    e.Message);
                return false;
            }
        }
    }

    public class ReadResearcherWithCategoriesCommand : IRequest<object>
    {
        public int Id { get; set; }
        public ReadResearcherWithCategoriesCommand(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            this.Id = id;
        }
    }
}
