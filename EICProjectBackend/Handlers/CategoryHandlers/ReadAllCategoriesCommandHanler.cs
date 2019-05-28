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

namespace EICProjectBackend.Handlers.CategoryHandlers
{ 
    public class ReadAllCategoriesCommandHanler : IRequestHandler<ReadAllCategoriesCommand, Object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Category> _categoryRepository;

        public ReadAllCategoriesCommandHanler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            this._unitOfWork = unitOfWork;
            this._categoryRepository = this._unitOfWork.CategoryRepository;
        }
        public async Task<object> Handle(ReadAllCategoriesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = this._categoryRepository.ReadAll().Result;
                if (result == null)
                    return null;

                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.CategoryReadAll,
                    ActivityTypes.Areas.ReadAllCategoriesHandler,
                    e.Message);
                return null;
            }
        }
    }

    public class ReadAllCategoriesCommand : IRequest<Object>
    {
        public ReadAllCategoriesCommand()
        {
            //only here for instantiating of the class
            //mediator related.
        }
    }
}
