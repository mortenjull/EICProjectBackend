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

namespace EICProjectBackend.Handlers.ProjectHandlers
{
    public class ReadAllProjectsHandler : IRequestHandler<ReadAllProjectsCommand, Object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Project> _projectRepository;

        public ReadAllProjectsHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            this._unitOfWork = unitOfWork;
            this._projectRepository = this._unitOfWork.ProjectRepository;
        }
        public async Task<object> Handle(ReadAllProjectsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = this._projectRepository.ReadAll().Result;
                if (result == null)
                    return null;

                return result;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ProjectReadAll,
                    ActivityTypes.Areas.ReadAllProjectsHandler,
                    e.Message);
                return null;
            }
        }
    }

    public class ReadAllProjectsCommand : IRequest<Object>
    {
        public ReadAllProjectsCommand()
        {
            //only here for instantiating of the class
            //mediator related.
        }
    }
}
