using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities.Entities;
using Infrastructur.Repositories;
using LoggingServices.LogItems.Activities;
using MediatR;
using Microsoft.AspNetCore.Http;
using UnitOfWork;

namespace EICProjectBackend.Handlers.TitleHandlers
{
    public class ReadAllTitleHandler : IRequestHandler<ReadAllTitleCommand, Object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<DBTitle> _titleRepository;

        public ReadAllTitleHandler(IUnitOfWork unitOfWork)
        {
            if(unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            this._unitOfWork = unitOfWork;
            this._titleRepository = this._unitOfWork.TitleRepository;          
        }
        public async Task<object> Handle(ReadAllTitleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._titleRepository.ReadAll();
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
                    ActivityTypes.Activities.TitleReadAll,
                    ActivityTypes.Areas.ReadAllTitlesHandler,
                    e.Message);
                return null;
            }
        }
    }

    public class ReadAllTitleCommand : IRequest<Object>
    {
        public ReadAllTitleCommand() { }
    }
}
