using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructur.Repositories;
using LoggingServices.LogItems.Activities;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnitOfWork;

namespace EICProjectBackend.Handlers.ProjectHandlers
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, Object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRelationsRepository _relationsRepository;
        private readonly IProjectRepository _projectRepository;

        public DeleteProjectHandler(IUnitOfWork unitOfWork)
        {
            if(unitOfWork == _unitOfWork)
                throw new ArgumentNullException(nameof(unitOfWork));

            this._unitOfWork = unitOfWork;
            this._relationsRepository = this._unitOfWork.RelationsRepository;
            this._projectRepository = this._unitOfWork.ProjectRepository;
        }

        public async Task<object> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var projOrgaResult = this._relationsRepository.DeleteProjectOrganisation(request.ProjectId);
                if (projOrgaResult == false)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }
                var projResearchResult = this._relationsRepository.DeleteProjectResearcherWithProjectId(request.ProjectId);
                if (projResearchResult == false)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }
                var projCatResult = this._relationsRepository.DeleteCategoriesProject(request.ProjectId);
                if (projCatResult == false)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }
                var result = this._projectRepository.Delete(request.ProjectId);
                if (result == null)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }
                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                this._unitOfWork.QueueUserActivityLogItem(ActivityTypes.Activities.ProjectDeleted, request.ProjectId);
                return true;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.ProjectDeleted,
                    ActivityTypes.Areas.DeleteProjectHandler,
                    e.Message);
                return null;
            }
        }
    }

    public class DeleteProjectCommand : IRequest<Object>
    {
        public DeleteProjectCommand(int id)
        {
            if(id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            this.ProjectId = id;
        }

        public int ProjectId { get; set; }
    }
}
