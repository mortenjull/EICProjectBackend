using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities;
using Infrastructur.Repositories;
using MediatR;
using UnitOfWork;

namespace EICProjectBackend.Handlers.ProjectHandlers
{
    public class ReadProjectHandler : IRequestHandler<ReadProjectCommand, Object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepository _projectRepository;
        public ReadProjectHandler(IUnitOfWork unitOfWork)
        {
            if(unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
            this._unitOfWork = unitOfWork;
            this._projectRepository = this._unitOfWork.ProjectRepository;
        }

        public async Task<object> Handle(ReadProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._projectRepository.ReadProjectWithChildren(request.ProjectId);
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
                return false;
            }
        }
    }

    public class ReadProjectCommand : IRequest<Object>
    {
        public ReadProjectCommand(int projectId)
        {
            if(projectId <= 0)
                throw new ArgumentOutOfRangeException(nameof(projectId));

            this.ProjectId = projectId;
        }

        public int ProjectId { get; set; }
    }
}
