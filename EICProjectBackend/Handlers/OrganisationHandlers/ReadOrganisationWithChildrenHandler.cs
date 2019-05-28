using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructur.Repositories;
using LoggingServices.LogItems.Activities;
using MediatR;
using UnitOfWork;

namespace EICProjectBackend.Handlers.OrganisationHandlers
{
    public class ReadOrganisationWithChildrenHandler : IRequestHandler<ReadOrganisationWithChildrenCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrganisationRepository _organisationRepository;

        public ReadOrganisationWithChildrenHandler(IUnitOfWork unitOfWork)
        {
            if(unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            this._unitOfWork = unitOfWork;
            this._organisationRepository = this._unitOfWork.OrganisationRepository;

        }

        public async Task<object> Handle(ReadOrganisationWithChildrenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._organisationRepository.ReadOrganisationWithChildren(request.OrganisationId);
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
                    ActivityTypes.Activities.OrganisationRead,
                    ActivityTypes.Areas.ReadOrganisationHandler,
                    e.Message);
                return false;
            }
        }
    }

    public class ReadOrganisationWithChildrenCommand : IRequest<object>
    {
        public ReadOrganisationWithChildrenCommand(int id)
        {
            if(id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            this.OrganisationId = id;
        }

        public int OrganisationId { get; private set; }
    }
}
