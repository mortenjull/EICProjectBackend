using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities;
using Domain.ModelEntities;
using EICProjectBackend.Services;
using Infrastructur.Repositories;
using LoggingServices.LogItems.Activities;
using Services;
using MediatR;
using UnitOfWork;

namespace EICProjectBackend.Handlers
{
    public class AuthSignupHandler : IRequestHandler<AuthSignupCommand, object>
    {
        private readonly IAuthService _authservice;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository<User> _userRepository;

        public AuthSignupHandler(IAuthService authservice, IUnitOfWork unitOfWork)
        {
            if(authservice == null)
                throw new ArgumentNullException(nameof(authservice));
            if(unitOfWork == null)
                throw new ArgumentNullException();
            this._authservice = authservice;
            this._unitOfWork = unitOfWork;
            this._userRepository = this._unitOfWork.UserRepository;
        }

        public async Task<object> Handle(AuthSignupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                User user = new User();

                user.UserName = request.Model.UserName;
                user.Created = DateTime.Now;
                user.RoleId = request.Model.Role.Id;
                user.Password = this._authservice.CalculateHash(request.Model.Password);

                var result = this._userRepository.Create(user);
                if (result == null)
                {
                    this._unitOfWork.Rollback();
                    return false;
                }

                this._unitOfWork.Commit();
                this._unitOfWork.Dispose();
                this._unitOfWork.QueueUserActivityLogItem(ActivityTypes.Activities.UserCreated, result.Id);
                return true;
            }
            catch (SqlException e)
            {
                if (e.Number == 2627)
                {
                    this._unitOfWork.Rollback();
                    return "The User already Exists";
                }
                this._unitOfWork.Rollback();
                return false;
            }
            catch (Exception e)
            {
                this._unitOfWork.Rollback();               
                this._unitOfWork.QueueErrorLogItem(
                    ActivityTypes.Activities.UserCreated,
                    ActivityTypes.Areas.AuthSignupHandler,
                    e.Message);
                return false;
            }
        }
    }

    public class AuthSignupCommand : IRequest<object>
    {
        public AuthSignupCommand(AuthSignupModel model)
        {
            this.Model = model;
        }

        public AuthSignupModel Model { get; private set; }
    }
}
