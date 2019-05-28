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

namespace EICProjectBackend.Handlers.OrganisationHandlers
{
    public class ReadAllOrganisationsHandler : IRequestHandler<ReadAllOrganisationsCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Organisation> _organisationRepository;

        public ReadAllOrganisationsHandler(IUnitOfWork unitOfWork)
        {
            if(unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            this._unitOfWork = unitOfWork;
            this._organisationRepository = this._unitOfWork.OrganisationRepository;
        }

        public async Task<object> Handle(ReadAllOrganisationsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._organisationRepository.ReadAll();

                if (result == null)
                {
                    this._unitOfWork.Rollback();
                    return null;
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                return result.ToList();
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.OrganisationReadAll,
                    ActivityTypes.Areas.ReadAllOrginsationsHandler,
                    e.Message);

                return false;
            }
                

        }
    }

    public class ReadAllOrganisationsCommand : IRequest<object>
    {
        public ReadAllOrganisationsCommand()
        {

        }
    }
}
