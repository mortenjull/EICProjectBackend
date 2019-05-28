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
    public class UpdateResearcherHandler : IRequestHandler<UpdateResearcherCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResearcherRepository<Researcher> _researcherRepository;
        private readonly IRelationsRepository _relationsRepository;

        public UpdateResearcherHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();
            this._unitOfWork = unitOfWork;
            this._researcherRepository = this._unitOfWork.ResearcherRepository;
            this._relationsRepository = this._unitOfWork.RelationsRepository;

        }

        public async Task<object> Handle(UpdateResearcherCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Researcher researcher = new Researcher();
                researcher.Id = request.Model.ResearcherId;
                researcher.ResearcherName = request.Model.ResearcherName;
                researcher.Mail = request.Model.Mail;
                researcher.EICColab = request.Model.EICColab;
                researcher.Phone = request.Model.Phone;
                researcher.Categories = request.Model.Categories;
                researcher.Created = DateTime.Now;

                var result = this._researcherRepository.Update(researcher);
                if (result == null)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }
                var result1 = this._relationsRepository.DeleteResearcherCategoryViaReseacherID(result.Id);
                if (result1 == false)
                {
                    this._unitOfWork.Rollback();
                    return null;
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
                this._unitOfWork.QueueUserActivityLogItem(ActivityTypes.Activities.ResearcherDeleted, request.Model.ResearcherId);
                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ResearcherDeleted,
                    ActivityTypes.Areas.UpdateResearcherHandler,
                    e.Message);
                return null;
            }
        }
    }

    public class UpdateResearcherCommand : IRequest<object>
    {
        public ResearcherModel Model { get; set; }
        public UpdateResearcherCommand(ResearcherModel model)
        {
            this.Model = model;
        }
    }
}
