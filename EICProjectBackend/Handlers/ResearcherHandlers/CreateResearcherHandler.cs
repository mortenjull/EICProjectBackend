using Domain.DBEntities;
using Domain.ModelEntities;
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
    public class CreateResearcherHandler : IRequestHandler<CreateResearcherCommand, object>
    { 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResearcherRepository<Researcher> _researcherRepository;
        private readonly IRelationsRepository _relationsRepository;

        public CreateResearcherHandler(IUnitOfWork unitOfWork) {
            if (unitOfWork == null)
                throw new ArgumentNullException(); 
            this._unitOfWork = unitOfWork;
            this._researcherRepository = this._unitOfWork.ResearcherRepository;
            this._relationsRepository = this._unitOfWork.RelationsRepository;

        }

        public async Task<object> Handle(CreateResearcherCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Researcher researcher = new Researcher();

                researcher.ResearcherName = request.Model.ResearcherName;
                researcher.Mail = request.Model.Mail;
                researcher.EICColab = request.Model.EICColab;
                researcher.Phone = request.Model.Phone;
                researcher.Categories = request.Model.Categories;
                researcher.Created = DateTime.Now;

                var result = this._researcherRepository.Create(researcher);
                if (result == null)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }

                foreach (var cat in request.Model.Categories)
                {
                    ResearcherCategory researcherCategory = new ResearcherCategory();
                    researcherCategory.Created = DateTime.Now;
                    researcherCategory.CategoryId = cat.Id;
                    researcherCategory.ResearcherId = result.Id;

                    var catResult = this._relationsRepository.CreateResearcherCategory(researcherCategory);
                    if (catResult == null)
                    {
                        this._unitOfWork.Rollback();
                        return false;
                    }
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                this._unitOfWork.QueueUserActivityLogItem(ActivityTypes.Activities.ResearcherCreated, result.Id);
                return result;
            }
            catch (SqlException e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ResearcherCreated,
                    ActivityTypes.Areas.CreateResearcherHandler,
                    e.Message);
                return false;
            }
        }
    }

    public class CreateResearcherCommand : IRequest<object>
    {
        public CreateResearcherCommand(ResearcherModel model)
        {
            this.Model = model;
        }

        public ResearcherModel Model { get; private set; }
    }
}
